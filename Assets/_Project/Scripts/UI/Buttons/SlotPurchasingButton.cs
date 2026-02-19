using UnityEngine;

public class SlotPurchasingButton : ButtonClickHandler<SlotPurchasingButton>
{
    [SerializeField] private InApp _inApp;

    public InApp InApp => _inApp;
}