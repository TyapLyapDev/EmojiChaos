using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunSelector : MonoBehaviour
{
    [SerializeField] private ShopCardInfos _gunInfos;

    public Gun Prefab { get; private set; }

    public float BulletSpeed { get; private set; }

    public float TimeReload { get; private set; }

    public void Init(IReadOnlyList<ShopCardItemButtonType> saves)
    {
        List<GunShopCardInfo> cardInfos = _gunInfos.CardInfos.OfType<GunShopCardInfo>().ToList();

        for (int i = 0; i < saves.Count; i++)
        {
            if (saves[i] == ShopCardItemButtonType.Selected)
            {
                GunShopCardInfo info = cardInfos[i];
                Prefab = info.Prefab;
                BulletSpeed = info.BulletSpeed;
                TimeReload = info.TimeReload;

                return;
            }
        }
    }
}