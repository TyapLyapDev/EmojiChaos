using System;
using UnityEngine;

public class LevelUiHandler : InitializingWithConfigBehaviour<LevelUiConfig>
{
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private PauseButton _pauseButton;
    [SerializeField] private ResumeButton _resumeButton;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private ExitToMenuButton _exitToMenuButton;

    private LevelUiConfig _config;

    private void OnDestroy()
    {
        _pauseButton.Clicked -= OnPauseClicked;
        _resumeButton.Clicked -= OnResumeClicked;
        _restartButton.Clicked -= OnRestartClicked;
        _exitToMenuButton.Clicked -= OnExitToMenuClicked;
    }

    protected override void OnInitialize(LevelUiConfig config)
    {
        _config = config;
        _pauseButton.Clicked += OnPauseClicked;
        _resumeButton.Clicked += OnResumeClicked;
        _restartButton.Clicked += OnRestartClicked;
        _exitToMenuButton.Clicked += OnExitToMenuClicked;
        _pausePanel.HideFast();
    }

    private void OnPauseClicked(PauseButton _)
    {
        _config.PauseSwitcher.SetPause();
        _pausePanel.Show();
    }

    private void OnResumeClicked(ResumeButton _)
    {
        _config.PauseSwitcher.SetResume();
        _pausePanel.HideFast();
    }

    private void OnRestartClicked(RestartButton button)
    {
        _pausePanel.HideFast();
        _config.PauseSwitcher.SetResume();
        _config.SceneLoader.ReloadCurrentScene();
    }

    private void OnExitToMenuClicked(ExitToMenuButton button)
    {
        _pausePanel.HideFast();
        _config.PauseSwitcher.SetResume();
        _config.SceneLoader.LoadScene(Constants.MenuSceneName);
    }
}
