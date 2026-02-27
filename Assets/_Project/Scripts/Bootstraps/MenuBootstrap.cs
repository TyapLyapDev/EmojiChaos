using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private MenuUIHandler _menuUIHandler;
    [SerializeField] private InApp _noAds;
    [SerializeField] private InApp _additionalSlot;

    private void Start()
    {
        YandexGameConnector.ConsumePurchases(_noAds, _additionalSlot);

        _menuUIHandler.Initialize(new MenuUiParam(
            new Saver(Utils.CalculateLevelCountInProject()), 
            SceneLoader.Instance));

        Audio.Music.PlayMenu();
    }
}