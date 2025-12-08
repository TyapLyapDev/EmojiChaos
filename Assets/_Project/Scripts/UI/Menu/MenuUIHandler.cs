using UnityEngine;

public class MenuUIHandler : InitializingWithConfigBehaviour<MenuUiConfig>
{
    [SerializeField] private LevelsPanel _levelsPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private ProgressResetterPanel _progressResetterPanel;
    [SerializeField] private DarkBackgroundPanel _darkBackgroundPanel;

    [SerializeField] private LevelsPanelOpenerButton _levelsPanelOpenerButton;
    [SerializeField] private LevelsPanelCloserButton _levelsPanelCloserButton;
    [SerializeField] private SettingsPanelOpenerButton _settingsPanelOpenerButton;
    [SerializeField] private SettingsPanelCloserButton _settingsPanelCloserButton;
    [SerializeField] private ProgressLevelOpenerButton _progressLevelOpenerButton;
    [SerializeField] private LeaderboardOpenerButton _leaderboardOpenerButton;
    [SerializeField] private ProgressResetOpenerButton _progressResetOpenerButton;
    [SerializeField] private ProgressResetAcceptButton _progressResetAcceptButton;
    [SerializeField] private ProgressResetCancelButton _progressResetCancelButton;

    private MenuUiConfig _config;

    private void OnDestroy()
    {
        if (_levelsPanel != null)
            _levelsPanel.LevelClicked -= OnLevelClicked;

        if (_levelsPanelOpenerButton != null)
            _levelsPanelOpenerButton.Clicked -= OnLevelsPanelOpenClicked;

        if (_levelsPanelCloserButton != null)
            _levelsPanelCloserButton.Clicked -= OnLevelsPanelCloseClicked;

        if (_settingsPanelOpenerButton != null)
            _settingsPanelOpenerButton.Clicked -= OnSettingsPanelOpenClicked;

        if (_settingsPanelCloserButton != null)
            _settingsPanelCloserButton.Clicked -= OnSettingsPanelCloseClicked;

        if (_progressLevelOpenerButton != null)
            _progressLevelOpenerButton.Clicked -= OnProgressLevelOpenerClicked;

        if(_progressResetOpenerButton != null)
            _progressResetOpenerButton.Clicked -= OnProgressResetOpenerClicked;

        if (_progressResetAcceptButton != null)
            _progressResetAcceptButton.Clicked -= OnProgresResetAcceptClicked;

        if (_progressResetCancelButton != null)
            _progressResetCancelButton.Clicked -= OnProgresResetCancelClicked;
    }

    protected override void OnInitialize(MenuUiConfig config)
    {
        _config = config;

        _levelsPanel.Initialize(_config.Saver);
        _settingsPanel.Initialize(_config.Saver);
        _progressResetterPanel.Initialize();
        _darkBackgroundPanel.Initialize();

        _levelsPanelOpenerButton.Initialize();
        _levelsPanelCloserButton.Initialize();
        _settingsPanelOpenerButton.Initialize();
        _settingsPanelCloserButton.Initialize();
        _progressLevelOpenerButton.Initialize();
        _leaderboardOpenerButton.Initialize();
        _progressResetAcceptButton.Initialize();
        _progressResetCancelButton.Initialize();
        _progressResetOpenerButton.Initialize();

        _levelsPanel.LevelClicked += OnLevelClicked;
        _levelsPanelOpenerButton.Clicked += OnLevelsPanelOpenClicked;
        _levelsPanelCloserButton.Clicked += OnLevelsPanelCloseClicked;
        _settingsPanelOpenerButton.Clicked += OnSettingsPanelOpenClicked;
        _settingsPanelCloserButton.Clicked += OnSettingsPanelCloseClicked;
        _progressLevelOpenerButton.Clicked += OnProgressLevelOpenerClicked;
        _progressResetAcceptButton.Clicked += OnProgresResetAcceptClicked;
        _progressResetCancelButton.Clicked += OnProgresResetCancelClicked;
        _progressResetOpenerButton.Clicked += OnProgressResetOpenerClicked;
    }

    private void OnLevelsPanelOpenClicked(LevelsPanelOpenerButton _)
    {
        _levelsPanel.Show();
        _darkBackgroundPanel.Show();
    }

    private void OnLevelsPanelCloseClicked(LevelsPanelCloserButton _)
    {
        _levelsPanel.Hide();
        _darkBackgroundPanel.Hide();
    }

    private void OnSettingsPanelOpenClicked(SettingsPanelOpenerButton _)
    {
        _darkBackgroundPanel.Show();
        _settingsPanel.Show();
    }

    private void OnSettingsPanelCloseClicked(SettingsPanelCloserButton _)
    {
        _darkBackgroundPanel.Hide();
        _settingsPanel.Hide();
    }

    private void OnProgressLevelOpenerClicked(ProgressLevelOpenerButton _)
    {
        int levelIndex = _config.Saver.LevelProgress;
        _config.Saver.SetSelectedLevel(levelIndex);
        _config.Saver.Save();
        SceneLoader.Instance.LoadScene(Constants.LevelSceneName);
    }

    private void OnProgressResetOpenerClicked(ProgressResetOpenerButton _)
    {
        _settingsPanel.Hide();
        _progressResetterPanel.Show();
    }

    private void OnProgresResetAcceptClicked(ProgressResetAcceptButton _)
    {
        _progressResetterPanel.Hide();
        _settingsPanel.Show();
        _settingsPanel.ResetProgress();
    }

    private void OnProgresResetCancelClicked(ProgressResetCancelButton _)
    {
        _progressResetterPanel.Hide();
        _settingsPanel.Show();
    }

    private void OnLevelClicked(int levelIndex)
    {
        _config.Saver.SetSelectedLevel(levelIndex);
        _config.Saver.Save();
        SceneLoader.Instance.LoadScene(Constants.LevelSceneName);
    }
}