using System;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private static readonly int s_faderInId = Animator.StringToHash("FaderIn");
    private static readonly int s_faderOutId = Animator.StringToHash("FaderOut");

    [SerializeField] private Animator _animator;
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool _isFading;
    private Action _fadedInCallBack;
    private Action _fadedOutCallBack;

    public void PlayFadeIn(Action fadedInCallback)
    {
        if (_isFading)
            return;

        _isFading = true;
        _fadedInCallBack = fadedInCallback;
        _animator.Play(s_faderInId, -1, 0f);
    }

    public void PlayFadeOut(Action fadedOutCallback)
    {
        if (_isFading)
            return;

        _isFading = true;
        _fadedOutCallBack = fadedOutCallback;
        _animator.Play(s_faderOutId, -1, 0f);
    }

    private void OnFadingInCompleted()
    {
        _isFading = false;
        _fadedInCallBack?.Invoke();
        _fadedInCallBack = null;
    }

    private void OnFadingOutCompleted()
    {
        _isFading = false;
        _fadedOutCallBack?.Invoke();
        _fadedOutCallBack = null;
    }
}