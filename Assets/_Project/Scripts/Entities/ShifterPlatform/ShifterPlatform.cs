using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ShifterPlatform : MonoBehaviour
{
    private Flipper _flipper;
    private Detector _detector;
    private Bubble _bubble;
    private Transform _flipperParent;
    private List<Car> _carsToShow;
    private readonly List<Car> _carsEnter = new();

    private void Awake()
    {
        _flipper = GetComponentInChildren<Flipper>(true);
        _detector = GetComponentInChildren<Detector>(true);
        _flipperParent = _flipper.transform;
        _bubble = GetComponentInChildren<Bubble>(true);
        _bubble.Disable();
        InitCarsToShow();
    }

    private IEnumerator Start()
    {
        _detector.PerformByForce();

        yield return null;

        ShowNextCar();
    }

    private void OnEnable()
    {
        _detector.Entered += OnEnter;
        _detector.Exited += OnExit;
        _flipper.Flipped += OnFlipped;
    }

    private void OnDisable()
    {
        _detector.Entered -= OnEnter;
        _detector.Exited -= OnExit;
        _flipper.Flipped -= OnFlipped;
    }

    private void InitCarsToShow()
    {
        _carsToShow = transform.GetComponentsInChildren<Car>().ToList();

        foreach (Car car in _carsToShow)
            car.SetActive(false);
    }

    private void ShowNextCar()
    {
        if (_carsEnter.Count != 0 || _carsToShow.Count == 0)
            return;

        Car carToShow = _carsToShow.First();
        _carsToShow.Remove(carToShow);
        carToShow.SetParent(_flipperParent);
        carToShow.transform.position = _flipperParent.position;
        carToShow.transform.Rotate(0, 0, 180, Space.World);
        carToShow.SetActive(true);
        _bubble.Enable();
        _flipper.Flip();
    }

    private void OnEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Car car))
            if (_carsEnter.Contains(car) == false)
                _carsEnter.Add(car);
    }

    private void OnExit(Collider collider)
    {
        if (collider.TryGetComponent(out Car car))
        {
            _carsEnter.Remove(car);

            if (car.transform.parent == _flipperParent)
                car.SetParent(null);
        }

        ShowNextCar();
    }

    private void OnFlipped() =>
        _bubble.Disable();
}