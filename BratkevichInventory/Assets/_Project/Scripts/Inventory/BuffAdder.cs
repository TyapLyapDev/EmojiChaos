using UnityEngine;

public class BuffAdder : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Sprite _bulletIco;
    [SerializeField] private Sprite _damageRed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _inventory.AddBuf(_bulletIco, KeyCode.F);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _inventory.AddBuf(_damageRed, KeyCode.F);
    }
}