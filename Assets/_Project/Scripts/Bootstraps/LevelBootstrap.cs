using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private LevelUiHandler _uiHandler;
    [SerializeField] private CarSkinReplacer _skinReplacer;
    [SerializeField] private EnemySelector _enemySelector;
    [SerializeField] private GunSelector _gunSelector;

    private Level _level;
    private Tutorial _tutorial;
    private readonly ServicesRegistry _services = new();

    private void Start()
    {
        CreateLevel();
        RegisterServices();
        InitializeComponents();
        StartRun();
    }

    private void OnDestroy() =>
        _services?.Dispose();

    private void CreateLevel()
    {
        Saver saver = new(Utils.CalculateLevelCountInProject());
        _services.Add(saver);

        int levelIndex = saver.SelectedLevel;
        LevelFinder finder = new ();
        Level levelPrefab = finder.Find(levelIndex);
        Factory<Level> levelFactory = new(levelPrefab);
        _level = levelFactory.Create();

        if (levelIndex == 0)
        {
            Tutorial tutorialPrefab = Resources.Load<Tutorial>(Constants.TutorialPath);
            _tutorial = Instantiate(tutorialPrefab);
        }

        Camera.main.backgroundColor = _level.BackgroundColor;
        _skinReplacer.Init(_level, saver.GetShopButtonTypes(ShopEntityItemType.Cars));
        _enemySelector.Init(saver.GetShopButtonTypes(ShopEntityItemType.Enemies), _level.Speed);
        _gunSelector.Init(saver.GetShopButtonTypes(ShopEntityItemType.Guns));
    }

    private void RegisterServices()
    {
        _services.Add(new CarMovementDirector(_skinReplacer.Speed));
        _services.Add(new BulletMovementDirector(_gunSelector.BulletSpeed));
        _services.Add(new CameraShaker());
        _services.Add(new PauseSwitcher());
        _services.Add(new CarSwipeStrategy() as ISwipeStrategy);
        _services.Add(new TypeColorRandomizer(new(_level.Colors), new(_level.Ids)));
        _services.Add(SceneLoader.Instance);

        PoolBuilder poolBuilder = new();
        _services.Add(poolBuilder);

        _services.Add(poolBuilder.Build(_level.BulletPrefab));
        _services.Add(poolBuilder.Build(_level.SmokeParticlePrefab));
        _services.Add(poolBuilder.Build(_level.BloodParticlePrefab));
        _services.Add(poolBuilder.Build(_level.StarBangParticlePrefab));
        _services.Add(poolBuilder.Build(_level.HitParticlePrefab));

        _services.Add(new ParticleShower(_services.Get<Pool<SmokeParticle>>(), _services.Get<Pool<BloodParticle>>(), _services.Get<Pool<StarBangParticle>>(), _services.Get<Pool<HitParticle>>()));
        _services.Add(poolBuilder.Build(_enemySelector.Prefab, new EnemyConfig(_level.EnemySplineContainer, _services.Get<ParticleShower>())));
        _services.Add(new StarsCounter(_level.Stars));
        _services.Add(new SlotReservator(_level.Slots, _services.Get<ISwipeStrategy>()));
        _services.Add(new EnemySpawner(_services.Get<Pool<Enemy>>(), _services.Get<TypeColorRandomizer>(), _enemySelector.Speed));
        _services.Add(new CrowdSpawnCoordinator(this, _services.Get<EnemySpawner>(), new(_level.Crowds), _level.Portal));
        _services.Add(new EnemyRegistryToAttack(_services.Get<EnemySpawner>()));
        _services.Add(new EnemiesMovementDirector(_services.Get<EnemySpawner>(), _level.Portal, _enemySelector.Speed));
        _services.Add(new EnemiesCounter(_services.Get<CrowdSpawnCoordinator>()));
        _services.Add(new CarMovementInitiator(_services.Get<SlotReservator>(), _services.Get<CarMovementDirector>()));
        _services.Add(new LevelStatsHandler(_services.Get<EnemiesMovementDirector>(), _services.Get<StarsCounter>(), _services.Get<EnemiesCounter>(), _services.Get<Saver>().SelectedLevel + 1));
    }

    private void InitializeComponents()
    {
        InitializeSlots();
        InitializeCars();
        InitializeStars();
        InitializeUIHandler();
        InitializeTutorial();
    }

    private void StartRun()
    {
        _services.Get<BulletMovementDirector>().Run();
        _services.Get<EnemiesMovementDirector>().Run();
        _services.Get<CrowdSpawnCoordinator>().Run();

        Audio.Music.PlayLevel();
    }

    private void InitializeSlots()
    {
        Pool<Bullet> bulletPool = _services.Get<Pool<Bullet>>();
        EnemyRegistryToAttack enemyRegistryToAttack = _services.Get<EnemyRegistryToAttack>();
        BulletMovementDirector bulletSpeedDirector = _services.Get<BulletMovementDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();

        foreach (Rack slot in _level.Slots)
        {
            Shooter shooter = new(bulletPool, enemyRegistryToAttack, bulletSpeedDirector);
            Gun gun = Instantiate(_gunSelector.Prefab);
            gun.Initialize(new GunConfig(shooter, particleShower, _gunSelector.TimeReload));
            slot.Initialize(new(gun));
        }
    }

    private void InitializeCars()
    {
        CarMovementDirector carSpeedDirector = _services.Get<CarMovementDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();
        MapSplineNodes mapSplineNodes = new(_level.CarSplineContainer);
        TypeColorRandomizer colorRandomizer = _services.Get<TypeColorRandomizer>();

        foreach (Car car in _skinReplacer.Cars)
            if (car != null)
                if (colorRandomizer.TryGetColor(car.Id, out Color color))
                    car.Initialize(new CarConfig(carSpeedDirector, particleShower, mapSplineNodes, color));
    }

    private void InitializeStars()
    {
        EnemiesMovementDirector enemySpeedDirector = _services.Get<EnemiesMovementDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();
        CameraShaker cameraShaker = _services.Get<CameraShaker>();

        foreach (Star star in _level.Stars)
            star.Initialize(new StarConfig(enemySpeedDirector, particleShower, cameraShaker));
    }

    private void InitializeUIHandler()
    {
        List<SlotPurchasingButton> slotPurchasingButtons = _level.Slots.Select(s => s.SlotPurchasingButton).ToList();

        _uiHandler.Initialize(new LevelUiConfig(
            _services.Get<PauseSwitcher>(),
            _services.Get<Saver>(),
            _services.Get<LevelStatsHandler>(),
            _services.Get<SceneLoader>(),
            slotPurchasingButtons));
    }
        
    private void InitializeTutorial()
    {
        if (_tutorial != null)
        {
            _tutorial.Initialize(new(
                _level.Portal.transform, 
                _services.Get<EnemySpawner>(),
                _skinReplacer.Cars[0],
                _services.Get<ISwipeStrategy>(),
                _services.Get<EnemiesMovementDirector>(),
                _level.Slots.ToArray(),
                _level.Stars.Last()));
        }
    }
}