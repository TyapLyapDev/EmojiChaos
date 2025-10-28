using System.Collections.Generic;
using UnityEngine;

public class InventorySelector : MonoBehaviour
{
    [SerializeField] private BuffApplier _buffApplier;

    private List<InventorySlot> _slots = new();
    private int _currentSelectedIndex = 0;

    private void OnEnable() =>
        _buffApplier.Applied += OnBuffApplied;

    private void OnDisable() =>
        _buffApplier.Applied -= OnBuffApplied;

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0f)
            return;

        if (_slots.Count == 0)
            return;

        _slots[_currentSelectedIndex].Unselect();

        if (scroll > 0)
        {
            _currentSelectedIndex--;
        }
        else
        {
            _currentSelectedIndex++;
        }

        Select();
    }

    private void Select()
    {
        if(_slots.Count == 0)
            return;

        if (_currentSelectedIndex < 0)
            _currentSelectedIndex = _slots.Count - 1;

        if (_currentSelectedIndex >= _slots.Count)
            _currentSelectedIndex = 0;

        _slots[_currentSelectedIndex].Select();
        _buffApplier.SetSelected(_slots[_currentSelectedIndex]);
    }

    public void AddSlot(InventorySlot slot)
    {
        _slots.Add(slot);

        if (_slots.Count == 1)
        {
            _currentSelectedIndex = 0;
            Select();
        }
    }

    private void OnBuffApplied(InventorySlot slot)
    {
        _slots.Remove(slot);

        _currentSelectedIndex--;

        if (_currentSelectedIndex < 0)
            _currentSelectedIndex = 0;

        Select();
        Destroy(slot.gameObject);
    }
}