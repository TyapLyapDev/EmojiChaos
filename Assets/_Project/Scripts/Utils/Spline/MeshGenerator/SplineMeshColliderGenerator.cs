using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(SplineContainer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SplineMeshColliderGenerator : MonoBehaviour
{
#if UNITY_EDITOR

    private const string DefaultMeshName = "UnknownMesh";
    private const string MeshAssetExtension = ".asset";
    private const string AssetsPathPrefix = "Assets/";
    private const float WidthDivider = 2f;
    private const float HeightDivider = 2f;

    [HideInInspector][SerializeField] private SplineContainer _splineContainer;
    [HideInInspector][SerializeField] private MeshCollider _meshCollider;
    [HideInInspector][SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private Level _level;
    [SerializeField] private float _width = 1;
    [SerializeField] private float _height = 1;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int[] _resolutions;
    [SerializeField] private bool _isAutoupdate = true;
    [SerializeField] private string _meshSavePath = "_Project/Art/GeneratedMeshes/";
    [SerializeField] private string _meshSuffixName = "_collider_mesh";
    [ReadOnly][SerializeField] private int _verticesCount;
    [ReadOnly][SerializeField] private int _trianglesCount;

    private bool _isSubscribedToSplineEvents = false;

    private void OnValidate()
    {
        if (_splineContainer == null)
            _splineContainer = GetComponent<SplineContainer>();

        if (_meshCollider == null)
            _meshCollider = GetComponent<MeshCollider>();

        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        if (_level == null)
            _level = GetComponentInParent<Level>();

        if (_rigidbody.isKinematic == false)
            _rigidbody.isKinematic = true;

        SplineUtils.SynchronizeArraySize<int>(ref _resolutions, _splineContainer.Splines.Count);

        for (int i = 0; i < _resolutions.Length; i++)
            _resolutions[i] = Mathf.Max(_resolutions[i], 0);

        if (_isAutoupdate)
        {
            SubscribeToSplineEvents();
            GenerateMesh();
        }
        else
        {
            UnsubscribeFromSplineEvents();
        }
    }

    private void OnEnable() =>
        SubscribeToSplineEvents();

    private void OnDisable() =>
        UnsubscribeFromSplineEvents();

    private void OnDestroy() =>
        UnsubscribeFromSplineEvents();

    private void SubscribeToSplineEvents()
    {
        if (_isAutoupdate == false)
            return;

        if (_isSubscribedToSplineEvents)
            return;

        Spline.Changed += OnSplineChanged;
        _isSubscribedToSplineEvents = true;
    }

    private void UnsubscribeFromSplineEvents()
    {
        if (_isSubscribedToSplineEvents == false)
            return;

        Spline.Changed -= OnSplineChanged;
        _isSubscribedToSplineEvents = false;
    }

    private void OnSplineChanged(Spline spline, int __, SplineModification ___)
    {
        if (_isAutoupdate && _splineContainer.Splines.Contains(spline))
            GenerateMesh();
    }

    private void GenerateMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();

        for (int splineIndex = 0; splineIndex < _splineContainer.Splines.Count; splineIndex++)
        {
            if (_resolutions != null && splineIndex < _resolutions.Length && _resolutions[splineIndex] < 2)
                continue;

            Spline spline = _splineContainer.Splines[splineIndex];
            int splineResolution = _resolutions[splineIndex];

            AddSplineToMesh(spline, splineResolution, vertices, triangles);
        }

        SaveMeshAsAsset(CreateMesh(vertices, triangles));
    }

    private void AddSplineToMesh(Spline spline, int resolution, List<Vector3> vertices, List<int> triangles)
    {
        int startIndex = vertices.Count;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            var (position, right, up) = GetSplineFrame(spline, t);

            vertices.AddRange(new[]
            {
                position - right - up,
                position + right - up,
                position - right + up,
                position + right + up
            });

            if (i > 0)
            {
                int curr = startIndex + i * 4;
                int prev = curr - 4;
                AddConnectionFaces(triangles, prev, curr);
            }
        }
    }

    private (Vector3 position, Vector3 right, Vector3 up) GetSplineFrame(Spline spline, float t)
    {
        Vector3 position = (Vector3)spline.EvaluatePosition(t) + _offset;
        Vector3 tangent = spline.EvaluateTangent(t);
        Vector3 right = Vector3.Cross(Vector3.up, tangent).normalized * _width / WidthDivider;
        Vector3 up = Vector3.up * _height / HeightDivider;
        return (position, right, up);
    }

    private void AddConnectionFaces(List<int> triangles, int prevBase, int currBase)
    {
        int[][] faces = new[]
        {
            new[] { prevBase + 0, currBase + 0, currBase + 2, prevBase + 2 }, // Left
            new[] { prevBase + 1, prevBase + 3, currBase + 3, currBase + 1 }, // Right  
            new[] { prevBase + 2, currBase + 2, currBase + 3, prevBase + 3 }, // Top
            new[] { prevBase + 0, prevBase + 1, currBase + 1, currBase + 0 }, // Bottom
            new[] { prevBase + 0, prevBase + 2, prevBase + 3, prevBase + 1 }, // Front (предыдущий)
            new[] { currBase + 0, currBase + 1, currBase + 3, currBase + 2 }  // Back (текущий)
        };

        foreach (var face in faces)
        {
            triangles.AddRange(new[] {
                face[0], face[1], face[2], // первый треугольник
                face[0], face[2], face[3]  // второй треугольник
            });
        }
    }

    private Mesh CreateMesh(List<Vector3> vertices, List<int> triangles)
    {
        Mesh mesh = new();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        _verticesCount = vertices.Count;
        _trianglesCount = triangles.Count;

        return mesh;
    }

    private void SaveMeshAsAsset(Mesh generatedMesh)
    {
        if (generatedMesh.vertexCount == 0)
        {
            _meshCollider.sharedMesh = null;
            return;
        }

        string path = $"{AssetsPathPrefix}{_meshSavePath}";
        string meshName = _level != null ? _level.gameObject.name : DefaultMeshName;
        meshName += _meshSuffixName;

        _meshCollider.sharedMesh = SplineUtils.SaveMeshAsAsset(generatedMesh, path, meshName, MeshAssetExtension);

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(_meshCollider);
    }

    [ContextMenu("Обновить меш коллайдера")]
    public void UpdateMeshCollider() =>
        GenerateMesh();

#endif
}