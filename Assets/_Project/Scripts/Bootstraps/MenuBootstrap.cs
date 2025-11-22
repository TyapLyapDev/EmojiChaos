using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private MenuUIHandler _menuUIHandler;

    private void Start()
    {
        _menuUIHandler.Initialize(new MenuUiConfig());
    }
}