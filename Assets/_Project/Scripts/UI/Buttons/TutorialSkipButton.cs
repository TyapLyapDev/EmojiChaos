using UnityEngine;

public class TutorialSkipButton: ButtonClickHandler<TutorialSkipButton>
{
    [SerializeField] private Tutorial _tutoril;

    private void Awake() =>
        Initialize();

    protected override void OnClick()
    {
        base.OnClick();
        _tutoril.Complete();
    }
}