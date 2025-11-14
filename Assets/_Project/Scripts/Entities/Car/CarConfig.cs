using System;
using UnityEngine;

public readonly struct CarConfig : IConfig
{
    private readonly CarSpeedDirector _speedDirector;
    private readonly ParticleShower _particleShower;
    private readonly MapSplineNodes _mapSplineNodes;
    private readonly Color _color;

    public CarConfig(CarSpeedDirector speedDirector, ParticleShower particleShower, MapSplineNodes mapSplineNodes, Color color)
    {
        _speedDirector = speedDirector ?? throw new ArgumentNullException(nameof(speedDirector));
        _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));
        _mapSplineNodes = mapSplineNodes ?? throw new ArgumentNullException(nameof(mapSplineNodes));
        _color = color;
    }

    public CarSpeedDirector SpeedDirector => _speedDirector;

    public ParticleShower ParticleShower => _particleShower;

    public MapSplineNodes MapSplineNodes => _mapSplineNodes;

    public Color Color => _color;
}