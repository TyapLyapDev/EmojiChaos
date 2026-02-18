using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSkinReplacer : MonoBehaviour
{
    [SerializeField] private ShopCardInfos _carInfos;

    private List<Car> _cars = new();
    private float _speed = 5;

    public IReadOnlyList<Car> Cars => _cars;

    public float Speed => _speed;

    public void Init(Level level, IReadOnlyList<ShopCardItemButtonType> saves)
    {
        List<Car> cars = level.GetComponentsInChildren<Car>(true).ToList();
        Replace(cars, saves);
    }

    private void Replace(List<Car> cars, IReadOnlyList<ShopCardItemButtonType> saves)
    {
        List<CarShopCardInfo> cardInfos = _carInfos.CardInfos.OfType<CarShopCardInfo>().ToList();

        for (int i = 0; i < saves.Count; i++)
        {
            if (saves[i] == ShopCardItemButtonType.Selected)
            {
                _speed = cardInfos[i].Speed;
                ReplaceCars(cars, cardInfos[i]);

                return;
            }
        }
    }

    private void ReplaceCars(List<Car> cars, CarShopCardInfo cardInfo)
    {
        if (cars == null)
            return;

        _cars.Clear();

        for (int i = 0; i <cars.Count; i++)
        {
            Car currentCar = cars[i];
            Car prefab = GetPrefab(currentCar.Visual, cardInfo);

            if (prefab != null)
                ReplaceCar(currentCar, prefab);
            else
                _cars.Add(currentCar);
        }
    }

    private Car GetPrefab(CarVisual carVisual, CarShopCardInfo cardInfo)
    {
        return carVisual switch
        {
            CarMiniTypeVisual => cardInfo.MiniCarPrefab,
            CarMiddleTypeVisual => cardInfo.MiddleCarPrefab,
            CarMaxTypeVisual => cardInfo.MaxCarPrefab,
            _ => null
        };
    }

    private void ReplaceCar(Car oldCar, Car prefab)
    {
        Car newCar = Instantiate(prefab, oldCar.transform.parent);
        newCar.gameObject.SetActive(false);

        newCar.SetPositionAndRotation(oldCar.transform.position, oldCar.transform.rotation);
        newCar.SetId(oldCar.Id);
        newCar.SetBulletCount(oldCar.BulletCount);
        newCar.gameObject.SetActive(oldCar.gameObject.activeSelf);

        _cars.Add(newCar);
        oldCar.MarkReplacement();
        Destroy(oldCar.gameObject);
    }
}