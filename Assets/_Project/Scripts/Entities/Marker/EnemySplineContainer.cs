using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class EnemySplineContainer : InitializingBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;

    public SplineContainer SplineContainer => GetSafeReference(_splineContainer);

    protected override void OnInitialize()
    {
        if (_splineContainer == null)
            throw new System.NullReferenceException(nameof(CarSplineContainer));
    }
}