using System.Collections.Generic;
using UnityEngine;

public class SlotAttack : MonoBehaviour
{
    [SerializeField] private bool _needAds;
    [SerializeField] private GameObject _ads;
    [SerializeField] private Gun _gun;
    [SerializeField] private GameObject _text;

    private bool _isAvailable = true;

    public bool IsAvailable => _isAvailable && _needAds == false;

    public void Initialize(Pool<Bullet> pool, List<Enemy> enemies)
    {
        _gun.Initialize(pool, enemies);
        _ads.SetActive(_needAds);
        _gun.gameObject.SetActive(_isAvailable == false);
        _text.SetActive(_isAvailable == false);
    }

    private void OnEnable()
    {
        _gun.Finished += OnShootFinished;
    }

    private void OnDisable()
    {
        _gun.Finished -= OnShootFinished;
    }

    public void Apply(int id, int bulletCount, Color color)
    {
        _isAvailable = false;
        _gun.Activate(id, bulletCount, color);
        _text.gameObject.SetActive(true);
    }

    private void OnShootFinished()
    {
        _gun.gameObject.SetActive(false);
        _isAvailable = true;
    }
}