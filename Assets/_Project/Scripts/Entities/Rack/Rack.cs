using System;
using UnityEngine;
using YG;

public class Rack : InitializingBehaviour
{
    [SerializeField] private GameObject _rackModel;
    [SerializeField] private GameObject _rackRuins;
    [SerializeField] private AdvertisingButton _advertisingButton;
    [SerializeField] private PurchasingButton _purchasingButton;
    [SerializeField] private Gun _gun;

    private bool _isReserved;

    public event Action<Gun> GunInstalled;

    public bool IsAvailable => 
        _gun.IsActiveSelf() == false
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

        if (_gun.IsActiveSelf() == false && _rackModel.activeSelf == false)
            return false;

        _gun.Activate(carType, bulletCount, color);
        ResetReservation();
        GunInstalled?.Invoke(_gun);

        return true;
    }

    protected override void OnInitialize()
    {
        _advertisingButton.Initialize();
        _purchasingButton.Initialize();

        if (_purchasingButton.IsActiveSelf() && YG2.saves.SavesData.IsPurchsingRack == true)
        {
            _purchasingButton.SetActive(false);
            SetRack();
        }

        _gun.SetActive(false);

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
        if (_purchasingButton.IsActiveSelf() && id == Constants.PurchasingRack)
        {
            _purchasingButton.SetActive(false);
            SetRack();

            YG2.saves.SavesData.IsPurchsingRack = true;
            YG2.SaveProgress();
        }
    }
}