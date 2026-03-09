using System;
using UnityEngine;

namespace EmojiChaos.UI.Tutorial
{

public abstract class TutorialItem : MonoBehaviour
{
    private TutorialParam _config;

    protected bool IsActivated { get; private set; }

    public event Action<TutorialItem> Deactivated;

    protected TutorialParam Config => _config;

    public void Initilize(TutorialParam config)
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
}
