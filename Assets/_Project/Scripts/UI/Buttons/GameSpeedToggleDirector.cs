using System.Linq;
using UnityEngine;

public class GameSpeedToggleDirector : MonoBehaviour
{
    [SerializeField] private GameSpeedSwitchButton[] _buttons;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deselectedColor;
    [SerializeField] private float _selectedAlpha;
    [SerializeField] private float _deselectedAlpha;

    private PauseSwitcher _pauseSwitcher;

    private void OnEnable()
    {
        foreach(GameSpeedSwitchButton button in _buttons)
            button.Clicked += OnClickButton;
    }

    private void OnDisable()
    {
        foreach (GameSpeedSwitchButton button in _buttons)
            button.Clicked -= OnClickButton;

        SelectButton(_buttons.First());
    }

    public void Initialize(PauseSwitcher pauseSwitcher)
    {
        _pauseSwitcher = pauseSwitcher;

        foreach (GameSpeedSwitchButton button in _buttons)
            button.Initialize();

        DeselectButtons();
        SelectButton(_buttons.First());
    }

    private void DeselectButtons()
    {
        foreach (GameSpeedSwitchButton button in _buttons)
        {
            button.SetColor(_deselectedColor);
            button.SetAlpha(_deselectedAlpha);
        }
    }

    private void SelectButton(GameSpeedSwitchButton button)
    {
        button.SetColor(_selectedColor);
        button.SetAlpha(_selectedAlpha);
        _pauseSwitcher.SetTimeScale(button.TimeScale);
    }

    private void OnClickButton(GameSpeedSwitchButton button)
    {
        DeselectButtons();        
        SelectButton(button);
    }
}