using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Car : MonoBehaviour
{
    [SerializeField] private CarVisual _visual;
    [SerializeField] private int _id;
    [SerializeField] private int _bulletCount;

    private bool _isInitialized;

    public event Action<Car> Clicked;

    public int Id => _id;

    public int BulletCount => _bulletCount;

    private void OnMouseDown() =>
        Clicked?.Invoke(this);

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

        _visual.Initialize();
        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        ValidateInitialization(nameof(SetColor));
        _visual.SetColor(color);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите {nameof(Initialize)}");
    }
}