using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class Level : InitializingBehaviour
{
    [SerializeField] private EnemySplineContainer _enemySplineContainer;
    [SerializeField] private CarSplineContainer _carSplineContainer;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private SmokeParticle _smokeParticlePrefab;
    [SerializeField] private BloodParticle _bloodParticlePrefab;
    [SerializeField] private HitParticle _hitParticlePrefab;
    [SerializeField] private ParticleSystem _portalParticle;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private List<Crowd> _crowds;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRandomSequence;

    private List<Rack> _slots;
    private List<Car> _cars;
    private List<Gun> _guns;
    private List<Crowd> _readyCrowds;
    private List<int> _ids;

    public SplineContainer EnemySplineContainer => GetSafeReference(_enemySplineContainer.SplineContainer);

    public SplineContainer CarSplineContainer => GetSafeReference(_carSplineContainer.SplineContainer);

    public Enemy EnemyPrefab => GetSafeReference(_enemyPrefab);

    public SmokeParticle SmokeParticlePrefab => GetSafeReference(_smokeParticlePrefab);

    public BloodParticle BloodParticlePrefab => GetSafeReference(_bloodParticlePrefab);

    public HitParticle HitParticlePrefab => GetSafeReference(_hitParticlePrefab);

    public ParticleSystem PortalParticle => _portalParticle;

    public Bullet BulletPrefab => GetSafeReference(_bulletPrefab);

    public IReadOnlyList<Color> Colors => GetSafeReference(_colors);

    public float Speed => GetSafeValue(_speed);

    public IReadOnlyList<Rack> Slots => GetSafeReference(_slots);

    public IReadOnlyList<Car> Cars => GetSafeReference(_cars);

    public IReadOnlyList<Gun> Guns => GetSafeReference(_guns);

    public IReadOnlyList<Crowd> Crowds => GetSafeReference(_readyCrowds);

    public IReadOnlyList<int> Ids => GetSafeReference(_ids);

    protected override void OnInitialize()
    {
        _slots = GetComponentsInChildren<Rack>(true).ToList();
        _cars = GetComponentsInChildren<Car>(true).ToList();
        _guns = GetComponentsInChildren<Gun>(true).ToList();

        _enemySplineContainer.Initialize();
        _carSplineContainer.Initialize();
        _ids = GetIds();

        PrepareCrowds();

        if (_colors == null || _colors.Count < _ids.Count)
            throw new Exception($"–азмер {nameof(_colors)} должен быть не меньше размера {nameof(_ids)}");
    }

    private List<int> GetIds()
    {
        IEnumerable<int> crowdIds = _crowds.Select(c => c.Id);
        IEnumerable<int> carIds = GetComponentsInChildren<Car>(true).Select(c => c.Id);

        return crowdIds.Concat(carIds).Distinct().ToList();
    }

    private void PrepareCrowds()
    {
        _readyCrowds = new(_crowds);

        if (_isRandomSequence)
            Utils.Shuffle(_readyCrowds);
    }
}