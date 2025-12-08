using UnityEngine;

public class Rack : InitializingBehaviour
{
    [SerializeField] private GameObject _rackModel;
    [SerializeField] private GameObject _rackRuins;
    [SerializeField] private RackVisual _visual;
    [SerializeField] private AdvertisingBox _advertisingBox;
    [SerializeField] private PurchasingBox _purchasingBox;
    [SerializeField] private Gun _gun;
    [SerializeField] private bool _needAds;
    [SerializeField] private bool _needPurchase;

    private bool _isReserved;

    public bool IsAvailable => _gun.IsActive == false
        && _needAds == false
        && _needPurchase == false
        && _isReserved == false;

    private void OnDestroy()
    {
        if (_gun != null)
            _gun.ShootingCompleted -= OnShootingCompleted;
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
        ValidateInit(nameof(TryActivateGun));

        if (_gun.IsActive || _needAds)
            return false;

        _gun.Activate(carType, bulletCount, color);
        _visual.ShowAppearance();
        ResetReservation();

        return true;
    }

    protected override void OnInitialize()
    {
        _advertisingBox.SetActive(_needAds);
        _purchasingBox.SetActive(_needPurchase);
        _rackModel.SetActive(_needPurchase == false && _needAds == false);
        _rackRuins.SetActive(_needPurchase || _needAds);
        _gun.SetActive(false);
        _gun.ShootingCompleted += OnShootingCompleted;
    }

    private void OnShootingCompleted()
    {
        if (IsAvailable)
            _visual.Twinkle();
    }
}