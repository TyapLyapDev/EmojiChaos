using System;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private float _speed;

    private SplineContainer _splineContainer;
    private Transform _transform;
    private float _distance;
    private float _offset;

    public event Action<Enemy> Deactivated;

    private void Update() =>
        Move();

    public void Initialize(SplineContainer splineContainer, float offset)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        _splineContainer = splineContainer;
        _transform = transform;
        _distance = 0f;
        _offset = offset;

        Vector3 startPosition = _splineContainer.EvaluatePosition(0f);
        _transform.position = ApplyOffset(startPosition, 0f, _offset);
    }

    public void Deactivate() =>
        Deactivated?.Invoke(this);

    private void Move()
    {
        _distance += _speed * Time.deltaTime;

        float normalizedDistance = _distance / _splineContainer.CalculateLength();

        if (normalizedDistance >= 1f)
        {
            Deactivate();
            return;
        }

        try
        {
            Vector3 position = _splineContainer.EvaluatePosition(normalizedDistance);
            _transform.position = ApplyOffset(position, normalizedDistance, _offset);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error moving along spline: {e.Message}");
            Deactivate();
        }
    }

    private Vector3 ApplyOffset(Vector3 position, float normalizedDistance, float offset)
    {
        if (offset == 0f)
            return position;

        // Получаем направление сплайна в текущей точке
        Vector3 tangent = _splineContainer.EvaluateTangent(normalizedDistance);
        Vector3 up = _splineContainer.EvaluateUpVector(normalizedDistance);

        // Вычисляем перпендикулярное направление (правостороннее смещение)
        Vector3 right = Vector3.Cross(tangent.normalized, up.normalized);

        // Применяем отступ
        return position + right * offset;
    }
}