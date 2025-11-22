using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Splines;

[ExecuteInEditMode]
[RequireComponent(typeof(SplineContainer))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SplineRendererMeshGenerator : MonoBehaviour
{
    private const string DefaultMeshName = "UnknownMesh";
    private const string MeshAssetExtension = ".asset";
    private const string AssetsPathPrefix = "Assets/";

    [HideInInspector][SerializeField] private SplineContainer _splineContainer;
    [HideInInspector][SerializeField] private MeshFilter _meshFilter;

    [SerializeField] private Level _level;
    [SerializeField] private Mesh _segmentMesh;
    [SerializeField] private VectorAxis _forwardAxis = VectorAxis.Y;
    [SerializeField] private VectorAxis _uvAxis = VectorAxis.Y;
    [SerializeField] private Vector3 _positionAdjustment;
    [SerializeField] private Quaternion _rotationAdjustment;
    [SerializeField] private Vector3 _scaleAdjustment = Vector3.one;
    [SerializeField] private float[] _uvResolutions;
    [SerializeField] private string _meshSavePath = "_Project/Art/GeneratedMeshes/";
    [SerializeField] private string _meshSuffixName = "_renderer_mesh";
    [SerializeField] private bool _uniformUVs = true;
    [SerializeField] private bool _isTwistMesh;
    [SerializeField] private bool _isAutoupdate;

    private bool _isSubscribedToSplineEvents;

    public Vector3 PositionAdjustment => _positionAdjustment;

    public Vector3 ScaleAdjustment => _scaleAdjustment;

    public event Action Changed;

    private void OnValidate()
    {
        if (_splineContainer == null)
            _splineContainer = GetComponent<SplineContainer>();

        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();

        if (_level == null)
            _level = GetComponentInParent<Level>();

        SplineUtils.SynchronizeArraySize<float>(ref _uvResolutions, _splineContainer.Splines.Count);

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
        if (Application.isPlaying) 
            return;

            if (_segmentMesh == null)
        {
            Debug.LogError("Меш-сегмент не назначен");
            return;
        }

        List<Vector3> combinedVertices = new();
        List<Vector3> combinedNormals = new();
        List<Vector2> combinedUVs = new();
        List<int>[] combinedSubmeshTriangles = new List<int>[_segmentMesh.subMeshCount];

        for (int i = 0; i < _segmentMesh.subMeshCount; i++)
            combinedSubmeshTriangles[i] = new();

        int combinedVertexOffset = 0;
        int splineCounter = 0;
        Mesh normalizedSegmentMesh = _segmentMesh.NormalizeMesh(_rotationAdjustment, _scaleAdjustment);

        foreach (Spline spline in _splineContainer.Splines)
        {
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            List<Vector2> uvs = new();

            List<BezierKnot> knots = new(spline.Knots);
            List<Quaternion> knotRotations = new();

            foreach (BezierKnot knot in knots)
                knotRotations.Add(knot.Rotation);

            List<int>[] submeshTriangles = new List<int>[normalizedSegmentMesh.subMeshCount];

            for (int i = 0; i < normalizedSegmentMesh.subMeshCount; i++)
                submeshTriangles[i] = new();

            int segmentCount = knots.Count - 1;

            if (spline.Closed)
                segmentCount++;

            List<float> segmentRatios = new();

            for (int i = 0; i < segmentCount; i++)
            {
                float splinePoint = _splineContainer.GetDistanceAlongSpline(splineCounter, knots[i % knots.Count].Position);
                float ratio = splinePoint / spline.GetLength();
                segmentRatios.Add(ratio);
            }

            segmentRatios.Add(1f);

            float meshBoundsDistance = Mathf.Abs(SplineUtils.GetRequiredAxis(normalizedSegmentMesh.bounds.size, _forwardAxis));
            List<float> vertexRatios = new();
            List<Vector3> vertexOffsets = new();

            foreach (Vector3 vertex in normalizedSegmentMesh.vertices)
            {
                Vector3 offset = SplineUtils.GetRequiredOffset(vertex, _forwardAxis);
                float ratio = Mathf.Abs(SplineUtils.GetRequiredAxis(vertex, _forwardAxis)) / meshBoundsDistance;

                vertexRatios.Add(ratio);
                vertexOffsets.Add(offset);
            }

            for (int i = 0; i < segmentCount; i++)
            {
                int counter = 0;

                foreach (Vector3 vector in normalizedSegmentMesh.vertices)
                {
                    float point = segmentRatios[i] + (vertexRatios[counter] * (segmentRatios[(i + 1) % segmentRatios.Count] - segmentRatios[i]));
                    point = Mathf.Clamp01(point);

                    Vector3 tangent = spline.EvaluateTangent(point);
                    Vector3 splinePosition = spline.EvaluatePosition(point);

                    if (tangent.sqrMagnitude < Mathf.Epsilon)
                        tangent = Vector3.forward;

                    int knotAIndex = i % knots.Count;
                    int knotBIndex = (i + 1) % knots.Count;

                    float t = vertexRatios[counter];
                    Quaternion twistRotation = Quaternion.Slerp(knotRotations[knotAIndex], knotRotations[knotBIndex], t);
                    Quaternion splineRotation = Quaternion.LookRotation(tangent.normalized, _isTwistMesh ? (twistRotation * Vector3.up) : Vector3.up);

                    Vector3 transformedPosition = splinePosition + splineRotation * vertexOffsets[counter];
                    vertices.Add(transformedPosition + _positionAdjustment);

                    counter++;
                }

                for (int j = 0; j < normalizedSegmentMesh.normals.Length; j++)
                {
                    Vector3 normal = normalizedSegmentMesh.normals[j];
                    float point = segmentRatios[i] + (vertexRatios[j] * (segmentRatios[(i + 1) % segmentRatios.Count] - segmentRatios[i]));
                    point = Mathf.Clamp01(point);

                    Vector3 tangent = spline.EvaluateTangent(point);

                    if (tangent.sqrMagnitude < Mathf.Epsilon)
                        tangent = Vector3.forward;

                    int knotAIndex = i % knots.Count;
                    int knotBIndex = (i + 1) % knots.Count;

                    float t = vertexRatios[j];
                    Quaternion twistRotation = Quaternion.Slerp(knotRotations[knotAIndex], knotRotations[knotBIndex], t);

                    Quaternion splineRotation = Quaternion.LookRotation(tangent.normalized, _isTwistMesh ? (twistRotation * Vector3.up) : Vector3.up);
                    Vector3 transformedNormal = splineRotation * normal;
                    normals.Add(transformedNormal);
                }

                for (int submeshIndex = 0; submeshIndex < normalizedSegmentMesh.subMeshCount; submeshIndex++)
                {
                    int[] submeshIndices = normalizedSegmentMesh.GetTriangles(submeshIndex);

                    for (int k = 0; k < submeshIndices.Length; k += 3)
                    {
                        combinedSubmeshTriangles[submeshIndex].Add(submeshIndices[k] + combinedVertexOffset);
                        combinedSubmeshTriangles[submeshIndex].Add(submeshIndices[k + 2] + combinedVertexOffset);
                        combinedSubmeshTriangles[submeshIndex].Add(submeshIndices[k + 1] + combinedVertexOffset);
                    }
                }

                for (int j = 0; j < normalizedSegmentMesh.uv.Length; j++)
                {
                    float point;
                    Vector2 uv = normalizedSegmentMesh.uv[j];

                    if (_uniformUVs)
                        point = segmentRatios[i] + (vertexRatios[j] * (segmentRatios[(i + 1) % segmentRatios.Count] - segmentRatios[i]));
                    else
                        point = (i / (float)segmentCount) + (vertexRatios[j] * (1 / (float)segmentCount));

                    Vector2 splineUV = SplineUtils.MakeUVs(uv, point, splineCounter, _uvAxis, _uvResolutions);
                    uvs.Add(splineUV);
                }

                combinedVertexOffset += normalizedSegmentMesh.vertexCount;
            }

            combinedVertices.AddRange(vertices);
            combinedNormals.AddRange(normals);
            combinedUVs.AddRange(uvs);

            splineCounter++;
        }

        Mesh generatedMesh = new()
        {
            name = DefaultMeshName,
            vertices = combinedVertices.ToArray(),
            normals = combinedNormals.ToArray(),
            uv = combinedUVs.ToArray(),
            subMeshCount = _segmentMesh.subMeshCount
        };

        for (int submeshIndex = 0; submeshIndex < _segmentMesh.subMeshCount; submeshIndex++)
            generatedMesh.SetTriangles(combinedSubmeshTriangles[submeshIndex].ToArray(), submeshIndex);

        generatedMesh.RecalculateBounds();
        generatedMesh.RecalculateNormals();
        generatedMesh.RecalculateTangents();

        SaveMeshAsAsset(generatedMesh);

        Changed?.Invoke();
    }

    private void SaveMeshAsAsset(Mesh generatedMesh)
    {
        string path = $"{AssetsPathPrefix}{_meshSavePath}";
        string meshName = _level != null ? _level.gameObject.name : DefaultMeshName;
        meshName += _meshSuffixName;

        EditorApplication.delayCall += () =>
        {
            if (this != null && _meshFilter != null)
            {
                _meshFilter.sharedMesh = SplineUtils.SaveMeshAsAsset(generatedMesh, path, meshName, MeshAssetExtension);
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(_meshFilter);
            }
        };
    }

    [ContextMenu("Обновить меш рендерера")]
    public void UpdateMeshCollider() =>
        GenerateMesh();
}
#endif

public enum VectorAxis { X, Y }