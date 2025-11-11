using System;
using UnityEngine;

public class AttackSlot : MonoBehaviour
{
    [SerializeField] private AttackSlotVisual _visual;
    [SerializeField] private AdvertisingBox _advertisingBox;
    [SerializeField] private Gun _gun;
    [SerializeField] private bool _needAds;

    private bool _isReserved;
    private bool _isInitialized;

    public bool IsAvailable => _gun.IsActive == false
        && _needAds == false
        && _isReserved == false;

    private void OnDestroy()
    {
        if (_gun != null)
            _gun.ShootingCompleted -= OnShootingCompleted;
    }

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

        if (_advertisingBox == null)
            throw new NullReferenceException(nameof(_advertisingBox));

        if (_gun == null)
            throw new NullReferenceException(nameof(_gun));

        _advertisingBox.SetActive(_needAds);
        _gun.SetActive(false);
        _gun.ShootingCompleted += OnShootingCompleted;
        _isInitialized = true;
    }

    public void SetReservation()
    {
        _isReserved = true;

        _visual.StopTwinkle();
    }

    public void ResetReservation()
    {
        _isReserved = false;

        if (IsAvailable)
            _visual.Twinkle();
    }

    public bool TryActivateGun(int carType, int bulletCount, Color color)
    {
        ValidateInitialization(nameof(TryActivateGun));

        if (_gun.IsActive || _needAds)
            return false;

        _gun.Activate(carType, bulletCount, color);
        ResetReservation();

        return true;
    }

    private void OnShootingCompleted()
    {
        if (IsAvailable)
            _visual.Twinkle();
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызван до инициализации!");
    }
}