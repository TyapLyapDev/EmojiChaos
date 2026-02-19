using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Shop/GunCardInfo")]
public class GunShopCardInfo : ShopCardInfo
{
    [SerializeField] private Gun _prefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _timeReload;

    public Gun Prefab => _prefab;

    public float BulletSpeed => _bulletSpeed;

    public float TimeReload => _timeReload;
}