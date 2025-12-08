using System;

public class SmokeParticle : OneShotParticle, IPoolable<SmokeParticle>
{
    public event Action<SmokeParticle> Deactivated;

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    protected override void OnCompleted() =>
        Deactivate();
}