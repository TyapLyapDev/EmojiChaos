using UnityEngine;

public class MenuUIHandler : InitializingWithConfigBehaviour<MenuUiConfig>
{
    [SerializeField] private LevelsPanel _levelsPanel;
    [SerializeField] private SettingsPanel _settingsPanel;

    [SerializeField] private LevelsPanelOpenerButton _levelsPanelOpenerButton;
    [SerializeField] private LevelsPanelCloserButton _levelsPanelCloserButton;
    [SerializeField] private SettingsPanelOpenerButton _settingsPanelOpenerButton;
    [SerializeField] private SettingsPanelCloserButton _settingsPanelCloserButton;

    private MenuUiConfig _config;

    private void OnDisable()
    {
        _levelsPanelOpenerButton.Clicked -= OnLevelsPanelOpenClicked;
        _levelsPanelCloserButton.Clicked -= OnLevelsPanelCloseClicked;
        _settingsPanelOpenerButton.Clicked -= OnSettingsPanelOpenClicked;
        _settingsPanelCloserButton.Clicked -= OnSettingsPanelCloseClicked;
    }

    protected override void OnInitialize(MenuUiConfig config)
    {
        _config = config;

        _levelsPanel.Initialize();
        _levelsPanel.HideFast();
        _settingsPanel.HideFast();

        _levelsPanelOpenerButton.Clicked += OnLevelsPanelOpenClicked;
        _levelsPanelCloserButton.Clicked += OnLevelsPanelCloseClicked;
        _settingsPanelOpenerButton.Clicked += OnSettingsPanelOpenClicked;
        _settingsPanelCloserButton.Clicked += OnSettingsPanelCloseClicked;
    }

    private void OnLevelsPanelOpenClicked(LevelsPanelOpenerButton _)
    {
        _levelsPanel.Show();
    }

    private void OnLevelsPanelCloseClicked(LevelsPanelCloserButton _)
    {
        _levelsPanel.HideFast();
    }

    private void OnSettingsPanelOpenClicked(SettingsPanelOpenerButton _)
    {
        _settingsPanel.Show();
    }

    private void OnSettingsPanelCloseClicked(SettingsPanelCloserButton _)
    {
        _settingsPanel.HideFast();
    }
}