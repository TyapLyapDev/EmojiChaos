using TMPro;
using UnityEngine;

public class DefeatPanel : PanelBase 
{
    [SerializeField] private LanguageTextWithParam _level;

    public void Activate(int level) =>
        _level.SetParam((level + 1).ToString());

    protected override void OnShow()
    {
        base.OnShow();
        Audio.Sfx.PlayFail();
    }
}