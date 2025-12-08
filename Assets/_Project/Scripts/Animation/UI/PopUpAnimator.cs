using System;
using UnityEngine;

public class PopUpAnimator : UiAnimator
{
    [SerializeField] private UiAnimation _enterAnimation;
    [SerializeField] private UiAnimation _exitAnimation;

    public void PlayEnter() =>
        Play(_enterAnimation);

    public void PlayExit(Action completed = null) => 
        Play(_exitAnimation, completed);
}