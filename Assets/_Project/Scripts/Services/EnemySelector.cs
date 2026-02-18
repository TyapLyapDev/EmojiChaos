using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    [SerializeField] private ShopCardInfos _enemyInfos;

    public Enemy Prefab { get; private set; }

    public float Speed { get; private set; }

    public void Init(IReadOnlyList<ShopCardItemButtonType> saves, float levelSpeed)
    {
        List<EnemyShopCardInfo> cardInfos = _enemyInfos.CardInfos.OfType<EnemyShopCardInfo>().ToList();

        for (int i = 0; i < saves.Count; i++)
        {
            if (saves[i] == ShopCardItemButtonType.Selected)
            {
                EnemyShopCardInfo info = cardInfos[i];
                Prefab = info.Prefab;
                Speed = info.Speed * levelSpeed;

                return;
            }
        }
    }
}