using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public static class SplineUtils
{
    public static void SynchronizeArraySize<T>(ref T[] sourceArray, int targetSize)
    {
        if (sourceArray.Length == targetSize)
            return;

        T[] tempArray = new T[targetSize];
        int minLength = Math.Min(sourceArray.Length, targetSize);

        for (int i = 0; i < minLength; i++)
            tempArray[i] = sourceArray[i];

        sourceArray = tempArray;
    }

    public static Mesh SaveMeshAsAsset(Mesh generatedMesh, string pathOnly, string meshName, string fileExtension)
    {
        if (System.IO.Directory.Exists(pathOnly) == false)
            System.IO.Directory.CreateDirectory(pathOnly);

        string meshPath = $"{pathOnly}{meshName}{fileExtension}";
        Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);

        if (existingMesh != null)
        {
            UpdateExistingMesh(existingMesh, generatedMesh);

            return existingMesh;
        }
        else
        {
            Mesh meshCopy = new() { name = $"{meshName}" };
            CopyMeshData(generatedMesh, meshCopy);
            AssetDatabase.CreateAsset(meshCopy, meshPath);
            AssetDatabase.SaveAssets();

            return meshCopy;
        }
    }

    public static float GetRequiredAxis(Vector3 vector, VectorAxis axis)
    {
        return axis switch
        {
            VectorAxis.X => vector.x,
            _ => vector.y,
        };
    }

    public static Vector3 GetRequiredOffset(Vector3 vector, VectorAxis axis)
    {
        return axis switch
        {
            VectorAxis.X => new Vector3(vector.y, vector.z, 0f),
            _ => new Vector3(vector.x, vector.z, 0f),
        };
    }

    public static Vector2 MakeUVs(Vector2 uv, float point, int splineCount, VectorAxis uvAxis, float[] uvResolutions)
    {
        if (uvResolutions.Length == 0)
        {
            Debug.LogError("Массив разрешений в UV пуст");

            return Vector2.zero;
        }

        return uvAxis switch
        {
            VectorAxis.X => new Vector2(point * uvResolutions[splineCount], uv.y),
            _ => new Vector2(uv.x, point * uvResolutions[splineCount]),
        };
    }

    public static float GetDistanceAlongSpline(this SplineContainer splineContainer, int index, Vector3 point, int samples = 100)
    {
        Spline spline = splineContainer.Splines[index];
        float closestDistance = float.MaxValue;
        float closestT = 0f;

        for (int i = 0; i <= samples; i++)
        {
            float t = i / (float)samples;
            float3 splinePoint = spline.EvaluatePosition(t);
            float distanceToSplinePoint = Vector3.Distance(point, splinePoint);

            if (distanceToSplinePoint < closestDistance)
            {
                closestDistance = distanceToSplinePoint;
                closestT = t;
            }
        }

        float distance = 0f;
        int segments = 1000;
        float3 previousPoint = spline.EvaluatePosition(0f);

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments * closestT;
            float3 splinePoint = spline.EvaluatePosition(t);

            distance += Vector3.Distance(previousPoint, splinePoint);
            previousPoint = splinePoint;
        }

        return distance;
    }

    public static Mesh NormalizeMesh(this Mesh mesh, Quaternion rotationAdjustment, Vector3 scaleAdjustment)
    {
        Mesh normalizedMesh = UnityEngine.Object.Instantiate(mesh);
        Vector3[] vertices = normalizedMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.Scale(vertices[i], scaleAdjustment);
            vertices[i] = rotationAdjustment * vertices[i];
        }

        normalizedMesh.vertices = vertices;

        Vector3[] normals = normalizedMesh.normals;

        for (int i = 0; i < normals.Length; i++)
            normals[i] = rotationAdjustment * normals[i];

        normalizedMesh.normals = normals;

        normalizedMesh.RecalculateBounds();
        normalizedMesh.RecalculateTangents();

        return normalizedMesh;
    }

    private static void UpdateExistingMesh(Mesh existingMesh, Mesh generatedMesh)
    {
        existingMesh.Clear();
        existingMesh.vertices = generatedMesh.vertices;
        existingMesh.normals = generatedMesh.normals;
        existingMesh.uv = generatedMesh.uv;
        existingMesh.colors = generatedMesh.colors;
        existingMesh.tangents = generatedMesh.tangents;
        existingMesh.subMeshCount = generatedMesh.subMeshCount;

        for (int i = 0; i < generatedMesh.subMeshCount; i++)
            existingMesh.SetTriangles(generatedMesh.GetTriangles(i), i);

        existingMesh.RecalculateBounds();
        existingMesh.RecalculateNormals();
        existingMesh.RecalculateTangents();

        EditorUtility.SetDirty(existingMesh);
    }

    private static void CopyMeshData(Mesh source, Mesh target)
    {
        target.Clear();
        target.vertices = source.vertices;
        target.normals = source.normals;
        target.uv = source.uv;
        target.colors = source.colors;
        target.tangents = source.tangents;
        target.subMeshCount = source.subMeshCount;

        for (int i = 0; i < source.subMeshCount; i++)
            target.SetTriangles(source.GetTriangles(i), i);

        target.RecalculateBounds();
        target.RecalculateNormals();
        target.RecalculateTangents();
    }
}