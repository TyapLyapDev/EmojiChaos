using System;
using UnityEngine;

public class CarConfig : IConfig
{
    private readonly MapSplineNodes _mapSplineNodes;
    private readonly Color _color;

    public CarConfig(MapSplineNodes mapSplineNodes, Color color)
    {
        _mapSplineNodes = mapSplineNodes ?? throw new ArgumentNullException(nameof(_mapSplineNodes));
        _color = color;
    }

    public MapSplineNodes MapSplineNodes => _mapSplineNodes;

    public Color Color => _color;
}