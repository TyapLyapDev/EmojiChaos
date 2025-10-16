using System;
using UnityEngine;

public class AttackSlot : MonoBehaviour
{
    [SerializeField] private GameObject _ads;
    [SerializeField] private Gun _gun;
    [SerializeField] private bool _needAds;

    private bool _isInitialized;

    public void Initialize(Pool<Bullet> pool, EnemyRegistry enemyRegistry, Action<Bullet> bulletActivated)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_ads == null)
            throw new NullReferenceException(nameof(_ads));

        if (_gun == null)
            throw new NullReferenceException(nameof(_gun));

        _gun.Initialize(pool, enemyRegistry, bulletActivated);
        _ads.SetActive(_needAds);
        _gun.gameObject.SetActive(false);

        _isInitialized = true;
    }

    public bool TryActivateGun(int carType, int bulletCount, Color color)
    {
        ValidateInitialization(nameof(TryActivateGun));

        if (_gun.IsActive || _needAds)
            return false;

        _gun.Activate(carType, bulletCount, color);

        return true;
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызван перед инициализацией. Сначала вызовите{nameof(Initialize)}");
    }
}