using System;
using UnityEngine;
using YG;

public class Rack : InitializingWithConfigBehaviour<RackConfig>
{
    [SerializeField] private GameObject _rackModel;
    [SerializeField] private GameObject _rackRuins;
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private AdvertisingButton _advertisingButton;
    [SerializeField] private SlotPurchasingButton _purchasingButton;
    
    private Gun _gun;

    private bool _isReserved;

    public event Action<Gun> GunInstalled;

    public SlotPurchasingButton SlotPurchasingButton => _purchasingButton;

    public bool IsAvailable => 
        _gun.IsAvailable
        && _isReserved == false
        && _rackModel.activeSelf;

    private void OnDestroy()
    {
        YG2.onRewardAdv -= OnRewardAdv;
        YG2.onPurchaseSuccess -= OnPurchaseSucces;
    }

    public void SetReservation() =>
        _isReserved = true;

    public void ResetReservation() =>
        _isReserved = false;

    public bool TryActivateGun(int carType, int bulletCount, Color color)
    {
        ValidateInit(nameof(TryActivateGun));

        if (_gun.IsAvailable == false || _rackModel.activeSelf == false)
            return false;

        _gun.Activate(carType, bulletCount, color);
        ResetReservation();
        GunInstalled?.Invoke(_gun);

        return true;
    }

    protected override void OnInitialize(RackConfig config)
    {
        _gun = config.Gun;
        _gun.SetParent(transform);
        _gun.SetPositionAndRotation(_gunTarget.position, _gunTarget.rotation);
        _gun.SetActive(false);

        _advertisingButton.Initialize();
        _purchasingButton.Initialize();

        if (_purchasingButton.IsActiveSelf() && YG2.saves.SavesData.IsPurchsingRack == true)
        {
            _purchasingButton.SetActive(false);
            SetRack();
        }

        YG2.onRewardAdv += OnRewardAdv;
        YG2.onPurchaseSuccess += OnPurchaseSucces;
    }

    private void SetRack()
    {
        _rackRuins.SetActive(false);
        _rackModel.SetActive(true);
    }

    private void OnRewardAdv(string id)
    {
        if (id == Constants.RewardRack + _advertisingButton.GetInstanceID())
        {
            _advertisingButton.SetActive(false);
            SetRack();
        }
    }

    private void OnPurchaseSucces(string id)
    {
        if (_purchasingButton.IsActiveSelf() && id == _purchasingButton.InApp.Id)
        {
            _purchasingButton.SetActive(false);
            SetRack();

            YG2.saves.SavesData.IsPurchsingRack = true;
            YG2.SaveProgress();
        }
    }
}

public readonly struct RackConfig : IConfig
{
    private readonly Gun _gun;

    public RackConfig(Gun gun)
    {
        _gun = gun != null ? gun : throw new ArgumentNullException(nameof(gun));
    }

    public Gun Gun => _gun;
}