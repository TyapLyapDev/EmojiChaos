using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameSpeedSwitchButton : ButtonClickHandler<GameSpeedSwitchButton>
{
    [SerializeField] private float _timeScale = 1f;

    private CanvasGroup _canvasGroup;

    public float TimeScale => _timeScale;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetAlpha(float value) =>
        _canvasGroup.alpha = value;
}