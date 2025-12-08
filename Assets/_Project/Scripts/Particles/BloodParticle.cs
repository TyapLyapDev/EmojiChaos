using System;
using UnityEngine;

public class BloodParticle : OneShotParticle, IPoolable<BloodParticle>
{
    [SerializeField] private ParticleSystem _stratched;
    [SerializeField] private ParticleSystem _circles;

    ParticleSystem.MainModule _module;
    ParticleSystem.MainModule _stratchedModule;
    ParticleSystem.MainModule _circlesModule;

    public event Action<BloodParticle> Deactivated;

    public void Deactivate()
    {
        ValidateInit(nameof(Deactivate));

        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        _module.startColor = color;
        _stratchedModule.startColor = color;
        _circlesModule.startColor = color;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _module = ParticleSystem.main;
        _stratchedModule = _stratched.main;
        _circlesModule = _circles.main;
    }

    protected override void OnCompleted() =>
        Deactivate();
}
