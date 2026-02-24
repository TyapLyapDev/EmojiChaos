using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUiHandler : InitializingWithConfigBehaviour<LevelUiConfig>
{
    private const float DelayAfterGameOver = 1.5f;

    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private VictoryPanel _victoryPanel;
    [SerializeField] private DefeatPanel _defeatPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private ProgressResetterPanel _progressResetterPanel;
    [SerializeField] private PurchasePanel _purchasePanel;
    [SerializeField] private DarkBackgroundPanel _darkBackgroundPanel;

    [SerializeField] private PauseButton _pauseButton;
    [SerializeField] private ResumeButton _resumeButton;
    [SerializeField] private NextLevelOpenerButton _nextLevelOpenerButton;
    [SerializeField] private SettingsPanelOpenerButton _settingsPanelOpenerButton;
    [SerializeField] private SettingsPanelCloserButton _settingsPanelCloserButton;
    [SerializeField] private List<RestartButton> _restartButtons;
    [SerializeField] private List<ExitToMenuButton> _exitToMenuButtons;
    [SerializeField] private ProgressResetOpenerButton _progressResetOpenerButton;
    [SerializeField] private ProgressResetAcceptButton _progressResetAcceptButton;
    [SerializeField] private ProgressResetCancelButton _progressResetCancelButton;
    [SerializeField] private GameSpeedToggleDirector _gameSpeedToggleDirector;
    [SerializeField] private PurchasePanelCloserButton _purchasePanelCloserButton;
    [SerializeField] private PurchaseApplierButton _purchaseApplierButton;

    [SerializeField] private SliderInformer _musicSlider;
    [SerializeField] private LanguageSwitchHandlerWithParam _levelTex;

    private LevelUiConfig _config;
    private Tween _delayedCallTween;

    private void OnDestroy()
    {
        _delayedCallTween?.Kill();

        if (_config.LevelStatsHandler != null)
        {
            _config.LevelStatsHandler.Victory -= OnVictory;
            _config.LevelStatsHandler.Defeat -= OnDefeat;
        }

        if (_pauseButton != null)
            _pauseButton.Clicked -= OnPauseClicked;

        if (_resumeButton != null)
            _resumeButton.Clicked -= OnResumeClicked;

        if (_nextLevelOpenerButton != null)
            _nextLevelOpenerButton.Clicked -= OnNextLevelClicked;

        if (_settingsPanelOpenerButton != null)
            _settingsPanelOpenerButton.Clicked -= OnOpenSettingsPanelClicked;

        if (_settingsPanelCloserButton != null)
            _settingsPanelCloserButton.Clicked -= OnCloseSettingsPanelClicked;

        if (_progressResetOpenerButton != null)
            _progressResetOpenerButton.Clicked -= OnProgressResetOpenerClicked;

        if (_progressResetAcceptButton != null)
            _progressResetAcceptButton.Clicked -= OnProgresResetAcceptClicked;

        if (_progressResetCancelButton != null)
            _progressResetCancelButton.Clicked -= OnProgresResetCancelClicked;

        if (_purchasePanelCloserButton != null)
            _purchasePanelCloserButton.Clicked -= OnPurchasePanelCloseClicked;

        if (_purchaseApplierButton != null)
            _purchaseApplierButton.Clicked -= OnPurchaseApplyClicked;

        foreach (RestartButton button in _restartButtons)
            if (button != null)
                button.Clicked -= OnRestartClicked;

        foreach (ExitToMenuButton button in _exitToMenuButtons)
            if (button != null)
                button.Clicked -= OnExitToMenuClicked;

        foreach (SlotPurchasingButton button in _config.SlotPurchasingButtons)
            if (button != null)
                button.Clicked -= OnSlotPurchasingButtonClicked;

        if (_musicSlider != null)
        {
            _musicSlider.PointerDownPressed -= OnMusicSliderDownPressed;
            _musicSlider.PointerUpPressed -= OnMusicSliderUpPressed;
        }
    }

    public void HideGameSpeedButtons() =>
        _gameSpeedToggleDirector.SetActive(false);

    protected override void OnInitialize(LevelUiConfig config)
    {
        _config = config;

        _darkBackgroundPanel.Initialize();
        _pausePanel.Initialize();
        _victoryPanel.Initialize();
        _defeatPanel.Initialize();
        _settingsPanel.Initialize(_config.Saver, _config.SceneLoader);
        _progressResetterPanel.Initialize();
        _purchasePanel.Initialize();

        _pauseButton.Initialize();
        _resumeButton.Initialize();
        _nextLevelOpenerButton.Initialize();
        _settingsPanelOpenerButton.Initialize();
        _settingsPanelCloserButton.Initialize();
        _progressResetAcceptButton.Initialize();
        _progressResetCancelButton.Initialize();
        _progressResetOpenerButton.Initialize();
        _purchasePanelCloserButton.Initialize();

        _gameSpeedToggleDirector.Initialize(config.PauseSwitcher);
        _levelTex.SetParam((config.Saver.SelectedLevel + 1).ToString());

        _config.LevelStatsHandler.Victory += OnVictory;
        _config.LevelStatsHandler.Defeat += OnDefeat;

        _pauseButton.Clicked += OnPauseClicked;
        _resumeButton.Clicked += OnResumeClicked;
        _nextLevelOpenerButton.Clicked += OnNextLevelClicked;
        _settingsPanelOpenerButton.Clicked += OnOpenSettingsPanelClicked;
        _settingsPanelCloserButton.Clicked += OnCloseSettingsPanelClicked;
        _progressResetAcceptButton.Clicked += OnProgresResetAcceptClicked;
        _progressResetCancelButton.Clicked += OnProgresResetCancelClicked;
        _progressResetOpenerButton.Clicked += OnProgressResetOpenerClicked;
        _purchasePanelCloserButton.Clicked += OnPurchasePanelCloseClicked;
        _purchaseApplierButton.Clicked += OnPurchaseApplyClicked;

        _musicSlider.PointerDownPressed += OnMusicSliderDownPressed;
        _musicSlider.PointerUpPressed += OnMusicSliderUpPressed;

        foreach (RestartButton button in _restartButtons)
        {
            button.Initialize();
            button.Clicked += OnRestartClicked;
        }

        foreach (ExitToMenuButton button in _exitToMenuButtons)
        {
            button.Initialize();
            button.Clicked += OnExitToMenuClicked;
        }

        foreach (SlotPurchasingButton button in _config.SlotPurchasingButtons)
            button.Clicked += OnSlotPurchasingButtonClicked;
    }

    private void OnVictory()
    {
        _config.Saver.TryIncreaseLevelProgress();
        _config.Saver.SetWhereMoreStarCount(_config.LevelStatsHandler.StarCount);
        _config.Saver.IncreaseScore(_config.LevelStatsHandler.FinalScore);
        _config.Saver.Save();

        if (_config.Saver.NextLevelExists)
            _nextLevelOpenerButton.Show();
        else
            _nextLevelOpenerButton.Hide();

        _gameSpeedToggleDirector.SetActive(false);

        _victoryPanel.Activate(
            _config.LevelStatsHandler.Score,
            _config.LevelStatsHandler.StarCount,
            _config.Saver.SelectedLevel);

        ShowPanelAfterDelay(_victoryPanel);

        Audio.Music.Pause();
    }

    private void OnDefeat()
    {
        _gameSpeedToggleDirector.SetActive(false);
        _defeatPanel.Activate(_config.Saver.SelectedLevel);
        ShowPanelAfterDelay(_defeatPanel);

        Audio.Music.Pause();
    }

    private void ShowPanelAfterDelay(PanelBase panel)
    {
        _delayedCallTween = DOVirtual.DelayedCall(DelayAfterGameOver, () =>
        {
            if (panel != null && panel.gameObject != null)
            {
                _darkBackgroundPanel.Show();
                panel.Show();
                _config.PauseSwitcher.SetPause();
            }
        }, false).SetUpdate(true);
    }

    private void OnPauseClicked(PauseButton _)
    {
        _config.PauseSwitcher.SetPause();
        _darkBackgroundPanel.Show();
        _pausePanel.Show();

        Audio.Music.Pause();
    }

    private void OnResumeClicked(ResumeButton _)
    {
        _config.PauseSwitcher.SetResume();
        _darkBackgroundPanel.Hide();
        _pausePanel.Hide();

        Audio.Music.UnPause();
    }

    private void OnNextLevelClicked(NextLevelOpenerButton _)
    {
        _config.PauseSwitcher.SetResume();
        int levelIndex = _config.Saver.SelectedLevel;
        _config.Saver.SetSelectedLevel(levelIndex + 1);
        _config.Saver.Save();

        SceneLoader.Instance.ReloadCurrentScene();
    }

    private void OnOpenSettingsPanelClicked(SettingsPanelOpenerButton _)
    {
        _pausePanel.Hide();
        _settingsPanel.Show();
    }

    private void OnCloseSettingsPanelClicked(SettingsPanelCloserButton _)
    {
        _settingsPanel.Hide();
        _pausePanel.Show();
    }

    private void OnProgressResetOpenerClicked(ProgressResetOpenerButton _)
    {
        _settingsPanel.Hide();
        _progressResetterPanel.Show();
        _config.PauseSwitcher.SetResume();
    }

    private void OnPurchasePanelCloseClicked(PurchasePanelCloserButton button)
    {
        _purchasePanel.Hide();
        _darkBackgroundPanel.Hide();
        _config.PauseSwitcher.SetResume();
    }

    private void OnPurchaseApplyClicked(PurchaseApplierButton button)
    {
        _purchasePanel.Hide();
        _darkBackgroundPanel.Hide();
        _config.PauseSwitcher.SetResume();
    }

    private void OnProgresResetAcceptClicked(ProgressResetAcceptButton _)
    {
        _progressResetterPanel.Hide();
        _settingsPanel.Show();
        _settingsPanel.ResetProgress();
        _config.PauseSwitcher.SetResume();
        _config.SceneLoader.LoadScene(Constants.MenuSceneName);
    }

    private void OnProgresResetCancelClicked(ProgressResetCancelButton _)
    {
        _progressResetterPanel.Hide();
        _settingsPanel.Show();
    }

    private void OnMusicSliderDownPressed() =>
        Audio.Music.UnPause();

    private void OnMusicSliderUpPressed() =>
        Audio.Music.Pause();

    private void OnRestartClicked(RestartButton _)
    {
        _config.PauseSwitcher.SetResume();
        _config.SceneLoader.ReloadCurrentScene();
    }

    private void OnExitToMenuClicked(ExitToMenuButton button)
    {
        _config.PauseSwitcher.SetResume();
        _config.SceneLoader.LoadScene(Constants.MenuSceneName);
    }

    private void OnSlotPurchasingButtonClicked(SlotPurchasingButton button)
    {
        _purchasePanel.SetInfo(button.InApp);
        _purchasePanel.Show();
        _darkBackgroundPanel.Show();
        _config.PauseSwitcher.SetPause();
    }
}