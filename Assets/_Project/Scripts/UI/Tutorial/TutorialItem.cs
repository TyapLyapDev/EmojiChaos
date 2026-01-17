using System;
using UnityEngine;

public abstract class TutorialItem : MonoBehaviour
{
    public event Action<TutorialItem> Deactivated;

    private TutorialConfig _config;

    protected bool IsActivated { get; private set; }

    protected TutorialConfig Config => _config;

    public void Initilize(TutorialConfig config)
    {
        _config = config;
        Hide();
        OnInitialized();
    }

    public void Activate()
    {
        if (IsActivated)
            return;

        IsActivated = true;
        OnActivated();
    }

    public void Deactivate()
    {
        if (IsActivated == false)
            return;

        IsActivated = false;
        OnDeactivated();
        Deactivated?.Invoke(this);
    }

    protected abstract void OnDeactivated();

    protected abstract void OnActivated();

    protected virtual void OnInitialized() { }

    public void Hide() =>
        gameObject.SetActive(false);

    public void Show() =>
        gameObject.SetActive(true);
}