using System;
using UnityEngine;

public class Rack : InitializingWithConfigBehaviour<RackParam>
{
    [SerializeField] private GameObject _rackModel;
    [SerializeField] private GameObject _rackRuins;
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private AdvertisingButton _advertisingButton;
    [SerializeField] private SlotPurchasingButton _purchasingButton;

    private Gun _gun;
    private RackParam _param;
    private bool _isReserved;

    public event Action<Gun> GunInstalled;

    public SlotPurchasingButton SlotPurchasingButton => _purchasingButton;

    public bool IsAvailable =>
        _gun.IsAvailable
        && _isReserved == false
        && _rackModel.activeSelf;

    private void OnDestroy()
    {
        YandexGameConnector.AdvRewarded -= OnRewardAdv;
        YandexGameConnector.SuccessPurchased -= OnPurchaseSucces;

        if (_advertisingButton != null)
            _advertisingButton.Clicked -= OnClickAdvButton;
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

    protected override void OnInitialize(RackParam param)
    {
        _param = param;
        _gun = param.Gun;
        _gun.SetParent(transform);
        _gun.SetPositionAndRotation(_gunTarget.position, _gunTarget.rotation);
        _gun.SetActive(false);

        _advertisingButton.Initialize();
        _purchasingButton.Initialize();

        if (_purchasingButton.IsActiveSelf() && _param.Saver.IsPurchsingRack)
        {
            _purchasingButton.SetActive(false);
            SetRack();
        }

        _advertisingButton.Clicked += OnClickAdvButton;

        YandexGameConnector.AdvRewarded += OnRewardAdv;
        YandexGameConnector.SuccessPurchased += OnPurchaseSucces;
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
            _param.Saver.SetPurchasingRackState(true);
            _purchasingButton.SetActive(false);
            SetRack();
        }
    }

    private void OnClickAdvButton(AdvertisingButton button) =>
        YandexGameConnector.RewardedAdvShow(Constants.RewardRack + button.GetInstanceID());
}