using System;
using System.Collections;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    private const float AngleStep = 180;

    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _axis;

    private Transform _transform;
    private Coroutine _coroutine;
    private float _currentAngle;

    public event Action Flipped;

    private void Awake() =>
        _transform = transform;

    public void Flip() =>
        _coroutine ??= StartCoroutine(FlipRoutine());

    private IEnumerator FlipRoutine()
    {
        _currentAngle = 0;

        while (_currentAngle < AngleStep)
        {
            Rotate();

            yield return null;
        }

        _coroutine = null;
        Flipped?.Invoke();
    }

    private void Rotate()
    {
        float rotationThisFrame = _speed * Time.deltaTime;

        if (_currentAngle + rotationThisFrame > AngleStep)
            rotationThisFrame = AngleStep - _currentAngle;

        _transform.Rotate(_axis, rotationThisFrame, Space.World);
        _currentAngle += rotationThisFrame;
    }
}
