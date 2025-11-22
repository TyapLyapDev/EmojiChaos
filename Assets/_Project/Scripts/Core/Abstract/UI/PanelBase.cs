using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public void HideFast()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnShow() { }
}