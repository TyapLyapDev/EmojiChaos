using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private MenuUIHandler _menuUIHandler;

    private void Start()
    {
        _menuUIHandler.Initialize(new MenuUiConfig(
            new Saver(Utils.CalculateLevelCountInProject())));

        Audio.Music.PlayMenu();
    }
}