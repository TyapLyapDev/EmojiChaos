using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot _prefab;
    [SerializeField] private Transform _content;
    [SerializeField] private InventorySelector _selector;

    public void AddBuf(Sprite sprite, KeyCode hotkey)
    {
        InventorySlot inventorySlot = Instantiate(_prefab, _content);
        inventorySlot.Init(sprite, hotkey);
        _selector.AddSlot(inventorySlot);
    }
}