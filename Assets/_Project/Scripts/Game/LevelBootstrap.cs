using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    private const string LevelName = "Level";

    private Level _level;
    private readonly ServicesRegistry _services = new();

    private void Start()
    {
        CreateLevel();
        RegisterServices();
        InitializeComponents();
        StartRun();        
    }

    private void OnDestroy()
    {
        _services?.Dispose();
    }

    private void CreateLevel()
    {
        int levelIndex = 0;
        Level levelPrefab = Resources.Load<Level>($"{LevelName}{levelIndex}");
        Factory<Level> levelFactory = new(levelPrefab);
        _level = levelFactory.Create();
    }

    private void RegisterServices()
    {
        _services.Add(new CarSpeedDirector());
        _services.Add(new BulletSpeedDirector());
        _services.Add(new CarSwipeStrategy() as ISwipeStrategy);
        _services.Add(new TypeColorRandomizer(new(_level.Colors), new(_level.Ids)));

        PoolBuilder poolBuilder = new();
        _services.Add(poolBuilder);

        _services.Add(poolBuilder.Build(_level.BulletPrefab));
        _services.Add(poolBuilder.Build(_level.SmokeParticlePrefab));
        _services.Add(poolBuilder.Build(_level.BloodParticlePrefab));
        _services.Add(new ParticleShower(_services.Get<Pool<SmokeParticle>>(), _services.Get<Pool<BloodParticle>>()));
        _services.Add(poolBuilder.Build(_level.EnemyPrefab, new EnemyConfig(_level.EnemySplineContainer, _services.Get<ParticleShower>())));

        _services.Add(new SlotReservator(_level.Slots, _services.Get<ISwipeStrategy>()));
        _services.Add(new EnemySpawner(_services.Get<Pool<Enemy>>(), _services.Get<TypeColorRandomizer>(), _level.Speed));
        _services.Add(new EnemyRegistryToAttack(_services.Get<EnemySpawner>()));
        _services.Add(new EnemySpeedDirector(_services.Get<EnemySpawner>(), _level.Speed));
        _services.Add(new CrowdSpawnCoordinator(this, _services.Get<EnemySpawner>(), new(_level.Crowds)));
        _services.Add(new CarMovementInitiator(_services.Get<SlotReservator>(), _services.Get<CarSpeedDirector>()));
    }

    private void InitializeComponents()
    {
        InitializeGuns();
        InitializeSlots();
        InitializeCars();
    }

    private void StartRun()
    {
        _services.Get<BulletSpeedDirector>().Run();
        _services.Get<EnemySpeedDirector>().Run();
        _services.Get<CrowdSpawnCoordinator>().Run();
    }

    private void InitializeSlots()
    {
        foreach (Rack slot in _level.Slots)
            slot.Initialize();
    }

    private void InitializeCars()
    {
        TypeColorRandomizer colorRandomizer = _services.Get<TypeColorRandomizer>();
        MapSplineNodes mapSplineNodes = new(_level.CarSplineContainer);

        foreach (Car car in _level.Cars)
            if (car != null)
                if (colorRandomizer.TryGetColor(car.Id, out Color color))
                    car.Initialize(new CarConfig(mapSplineNodes, color));
    }

    private void InitializeGuns()
    {
        Pool<Bullet> bulletPool = _services.Get<Pool<Bullet>>();
        EnemyRegistryToAttack enemyRegistryToAttack = _services.Get<EnemyRegistryToAttack>();
        BulletSpeedDirector bulletSpeedDirector = _services.Get<BulletSpeedDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();

        foreach (Gun gun in _level.Guns)
        {
            Shooter shooter = new(bulletPool, enemyRegistryToAttack, bulletSpeedDirector);
            gun.Initialize(new GunConfig(shooter, particleShower));
        }
    }
}