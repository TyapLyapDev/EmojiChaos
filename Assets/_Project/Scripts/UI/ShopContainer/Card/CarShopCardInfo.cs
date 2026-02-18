using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Shop/CarCardInfo")]
public class CarShopCardInfo : ShopCardInfo
{
    [SerializeField] private Car _miniCarPrefab;
    [SerializeField] private Car _middleCarPrefab;
    [SerializeField] private Car _maxCarPrefab;
    [SerializeField] private float _speed;

    public Car MiniCarPrefab => _miniCarPrefab;

    public Car MiddleCarPrefab => _middleCarPrefab;

    public Car MaxCarPrefab => _maxCarPrefab;

    public float Speed => _speed;
}