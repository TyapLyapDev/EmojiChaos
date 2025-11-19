using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplinePositionFinder
{
    private readonly SplineContainer _splineContainer;
    private readonly Transform _splineContainerTransform;

    public SplinePositionFinder(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer != null ? splineContainer : throw new ArgumentNullException(nameof(splineContainer));
        _splineContainerTransform = splineContainer.transform;
    }

    public bool TryFindNearestProgressOnSpline(Spline spline, Vector3 worldPosition, out float progressOnSpline)
    {
        progressOnSpline = 0f;

        if (spline == null)
            return false;

        Vector3 localPosition = _splineContainerTransform.InverseTransformPoint(worldPosition);
        Ray ray = new(localPosition + Vector3.up * 10f, Vector3.down);

        SplineUtility.GetNearestPoint(spline, ray, out float3 _, out float tempProgress);
        progressOnSpline = Mathf.Clamp01(tempProgress);

        return true;
    }

    public bool TryFindNearestProgressOnAnySpline(Vector3 worldPosition, out float progressOnSpline, out Spline nearestSpline)
    {
        progressOnSpline = 0;
        nearestSpline = null;
        bool isFound = false;

        Vector3 localPosition = _splineContainerTransform.InverseTransformPoint(worldPosition);
        Ray ray = new(localPosition + Vector3.up * 10f, Vector3.down);
        float minimumDistance = float.MaxValue;

        foreach (Spline tempSpline in _splineContainer.Splines)
        {
            SplineUtility.GetNearestPoint(tempSpline, ray, out float3 point, out float tempProgress);
            float tempDistance = Vector3.Distance(localPosition, point);

            if (tempDistance < minimumDistance)
            {
                isFound = true;
                minimumDistance = tempDistance;
                nearestSpline = tempSpline;
                progressOnSpline = Mathf.Clamp01(tempProgress);
            }
        }

        return isFound;
    }

    public Vector3 GetWorldPosition(Spline spline, float progress)
    {
        float normalizedProgress = spline.Closed ? Mathf.Repeat(progress, 1f) : Mathf.Clamp01(progress);
        Vector3 localPosition = spline.EvaluatePosition(normalizedProgress);

        return _splineContainerTransform.TransformPoint(localPosition);
    }
}