using UnityEngine;

public class TutorialSkipButton: ButtonClickHandler<TutorialSkipButton>
{
    [SerializeField] private Tutorial _tutorial;

    private void Awake() =>
        Initialize();

    protected override void OnClick()
    {
        base.OnClick();
        _tutorial.Complete();
    }
}