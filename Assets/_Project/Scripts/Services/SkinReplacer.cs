using System.Collections.Generic;
using UnityEngine;

public class SkinReplacer : MonoBehaviour
{
    [Header ("LittleCars")]
    [SerializeField] private Car _littleMiniCarPrefab;
    [SerializeField] private Car _littleMiddleCarPrefab;
    [SerializeField] private Car _littleMaxCarPrefab;

    private readonly List<Car> _cars = new();

    public IReadOnlyList<Car> Cars => _cars;

    public void ReplaceCarsLittle(List<Car> cars)
    {
        for (int i = cars.Count - 1; i >= 0; i--)
        {
            Car car = cars[i];

            if (car.Visual is CarMiniTypeVisual _)
            {
                Car newCar = Instantiate(_littleMiniCarPrefab, car.transform.parent);
                newCar.SetPositionAndRotation(car.transform.position, car.transform.rotation);
                newCar.SetId(car.Id);
                newCar.SetBulletCount(car.BulletCount);

                _cars.Add(newCar);
                Destroy(car.gameObject);
            }
            else if (car.Visual is CarMiddleTypeVisual _)
            {
                Car newCar = Instantiate(_littleMiddleCarPrefab, car.transform.parent);
                newCar.SetPositionAndRotation(car.transform.position, car.transform.rotation);
                newCar.SetId(car.Id);
                newCar.SetBulletCount(car.BulletCount);

                _cars.Add(newCar);
                Destroy(car.gameObject);
            }
            else if (car.Visual is CarMaxTypeVisual _)
            {
                Car newCar = Instantiate(_littleMaxCarPrefab, car.transform.parent);
                newCar.SetPositionAndRotation(car.transform.position, car.transform.rotation);
                newCar.SetId(car.Id);
                newCar.SetBulletCount(car.BulletCount);

                _cars.Add(newCar);
                Destroy(car.gameObject);
            }
            else
            {
                _cars.Add(car);
            }
        }
    }
}