using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Car : MonoBehaviour
{
    [SerializeField] private CarVisual _visual;
    [SerializeField] private int _id;
    [SerializeField] private int _bulletCount;

    private Color _color;

    public Color Color => _color;

    public event Action<Car> Clicked;

    public int Id => _id;

    public int BulletCount => _bulletCount;

    public void Initialize() =>
        _visual.Initialize();

    public void SetColor(Color color)
    {
        _visual.SetColor(color);
        _color = color;
    }

    private void OnMouseDown() =>
        Clicked?.Invoke(this);
}