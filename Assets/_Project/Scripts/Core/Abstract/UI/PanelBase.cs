using UnityEngine;

public abstract class PanelBase : InitializingBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private PopUpAnimator _popUpAnimator;

    public void Show()
    {
        gameObject.SetActive(true);
        _popUpAnimator.PlayEnter();
        _canvasGroup.blocksRaycasts = true;
        OnShow();
    }

    public void Hide()
    {
        _canvasGroup.blocksRaycasts = false;
        _popUpAnimator.PlayExit(HideInstantly);
        OnHide();
    }

    protected override void OnInitialize()
    {
        _popUpAnimator.Initialize();
        HideInstantly();
    }

    protected virtual void OnShow() =>
        Audio.Sfx.PlayPanelShowed();

    protected virtual void OnHide() =>
        Audio.Sfx.PlayPanelClosed();

    private void HideInstantly() =>
        gameObject.SetActive(false);
}