using System;
using UnityEngine;

public class BuffApplier : MonoBehaviour
{
    private InventorySlot _slot;

    public event Action<InventorySlot> Applied;

    public void SetSelected(InventorySlot slot) =>
        _slot = slot;

    private void Update()
    {
        if(_slot == null)
            return;

        if (Input.GetKeyDown(_slot.Hotkey))
            Applied?.Invoke(_slot);
    }
}