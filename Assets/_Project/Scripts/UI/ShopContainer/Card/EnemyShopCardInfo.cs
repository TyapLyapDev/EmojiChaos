using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Shop/EnemyCardInfo")]
public class EnemyShopCardInfo : ShopCardInfo
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _speed;

    public Enemy Prefab => _prefab;

    public float Speed => _speed;
}