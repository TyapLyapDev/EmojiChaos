using UnityEngine;
using YG;

public class MenuUIHandler : InitializingWithConfigBehaviour<MenuUiConfig>
{
    [SerializeField] private LevelsPanel _levelsPanel;
    [SerializeField] private ShopPanel _shopPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private ProgressResetterPanel _progressResetterPanel;
    [SerializeField] private LeaderBoardPanel _leaderBoardPanel;
    [SerializeField] private DarkBackgroundPanel _darkBackgroundPanel;

    [SerializeField] private LevelsPanelOpenerButton _levelsPanelOpenerButton;
    [SerializeField] private LevelsPanelCloserButton _levelsPanelCloserButton;
    [SerializeField] private ShopPanelOpenerButton _shopPanelOpenerButton;
    [SerializeField] private ShopPanelCloserButton _shopPanelCloserButton;
    [SerializeField] private SettingsPanelOpenerButton _settingsPanelOpenerButton;
    [SerializeField] private SettingsPanelCloserButton _settingsPanelCloserButton;
    [SerializeField] private ProgressLevelOpenerButton _progressLevelOpenerButton;
    [SerializeField] private LeaderboardOpenerButton _leaderboardOpenerButton;
    [SerializeField] private LeaderboardPanelCloserButton _leaderboardCloserButton;
    [SerializeField] private NoAdsButton _noAdsButton;
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

        if (_shopPanelOpenerButton != null)
            _shopPanelOpenerButton.Clicked -= OnShopPanelOpenClicked;

        if (_shopPanelCloserButton != null)
            _shopPanelCloserButton.Clicked -= OnShopPanelCloseClicked;

        if (_settingsPanelOpenerButton != null)
            _settingsPanelOpenerButton.Clicked -= OnSettingsPanelOpenClicked;

        if (_settingsPanelCloserButton != null)
            _settingsPanelCloserButton.Clicked -= OnSettingsPanelCloseClicked;

        if (_progressLevelOpenerButton != null)
            _progressLevelOpenerButton.Clicked -= OnProgressLevelOpenClicked;

        if (_leaderboardOpenerButton != null)
            _leaderboardOpenerButton.Clicked -= OnLeaderboardOpenClicked;

        if (_leaderboardCloserButton != null)
            _leaderboardCloserButton.Clicked -= OnLeaderboardCloseClicked;

        if(_progressResetOpenerButton != null)
            _progressResetOpenerButton.Clicked -= OnProgressResetOpenerClicked;

        if (_progressResetAcceptButton != null)
            _progressResetAcceptButton.Clicked -= OnProgresResetAcceptClicked;

        if (_progressResetCancelButton != null)
            _progressResetCancelButton.Clicked -= OnProgresResetCancelClicked;

        YG2.onPurchaseSuccess -= OnPurchaseSuccess;
    }

    protected override void OnInitialize(MenuUiConfig config)
    {
        _config = config;

        _levelsPanel.Initialize(_config.Saver.LevelProgress, _config.Saver.GetStarInfos());
        _shopPanel.Initialize(_config.Saver);
        _settingsPanel.Initialize(_config.Saver, _config.SceneLoader);
        _progressResetterPanel.Initialize();
        _leaderBoardPanel.Initialize();
        _darkBackgroundPanel.Initialize();

        _levelsPanelOpenerButton.Initialize();
        _levelsPanelCloserButton.Initialize();
        _shopPanelOpenerButton.Initialize();
        _shopPanelCloserButton.Initialize();
        _settingsPanelOpenerButton.Initialize();
        _settingsPanelCloserButton.Initialize();
        _progressLevelOpenerButton.Initialize();
        _leaderboardOpenerButton.Initialize();
        _leaderboardCloserButton.Initialize();
        _noAdsButton.Initialize();
        _progressResetAcceptButton.Initialize();
        _progressResetCancelButton.Initialize();
        _progressResetOpenerButton.Initialize();

        _levelsPanel.LevelClicked += OnLevelClicked;
        _levelsPanelOpenerButton.Clicked += OnLevelsPanelOpenClicked;
        _levelsPanelCloserButton.Clicked += OnLevelsPanelCloseClicked;
        _shopPanelOpenerButton.Clicked += OnShopPanelOpenClicked;
        _shopPanelCloserButton.Clicked += OnShopPanelCloseClicked;
        _settingsPanelOpenerButton.Clicked += OnSettingsPanelOpenClicked;
        _settingsPanelCloserButton.Clicked += OnSettingsPanelCloseClicked;
        _progressLevelOpenerButton.Clicked += OnProgressLevelOpenClicked;
        _leaderboardOpenerButton.Clicked += OnLeaderboardOpenClicked;
        _leaderboardCloserButton.Clicked += OnLeaderboardCloseClicked;
        _progressResetAcceptButton.Clicked += OnProgresResetAcceptClicked;
        _progressResetCancelButton.Clicked += OnProgresResetCancelClicked;
        _progressResetOpenerButton.Clicked += OnProgressResetOpenerClicked;

        YG2.onPurchaseSuccess += OnPurchaseSuccess;

        bool isNoAds = YG2.saves.SavesData.IsNoAds;
        YG2.StickyAdActivity(isNoAds == false);
        _noAdsButton.SetActive(isNoAds == false);
        _noAdsButton.transform.parent.gameObject.SetActive(isNoAds == false);
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

    private void OnShopPanelOpenClicked(ShopPanelOpenerButton button)
    {
        _shopPanel.Show();
        _darkBackgroundPanel.Show();
    }

    private void OnShopPanelCloseClicked(ShopPanelCloserButton button)
    {
        _shopPanel.Hide();
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

    private void OnProgressLevelOpenClicked(ProgressLevelOpenerButton _)
    {
        int levelIndex = Mathf.Min(_config.Saver.LevelProgress, _config.Saver.TotalLevelsCount - 1);
        _config.Saver.SetSelectedLevel(levelIndex);
        _config.Saver.Save();
        SceneLoader.Instance.LoadScene(Constants.LevelSceneName);
    }

    private void OnLeaderboardOpenClicked(LeaderboardOpenerButton _)
    {
        _darkBackgroundPanel.Show();
        _leaderBoardPanel.Show();
    }

    private void OnLeaderboardCloseClicked(LeaderboardPanelCloserButton _)
    {
        _darkBackgroundPanel.Hide();
        _leaderBoardPanel.Hide();
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

        _config.SceneLoader.LoadScene(Constants.MenuSceneName);
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

    private void OnPurchaseSuccess(string id)
    {
        if(id == Constants.PurchasingNoAds)
        {
            _config.Saver.DisableAds();

            YG2.StickyAdActivity(false);
            _noAdsButton.SetActive(false);
            _noAdsButton.transform.parent.gameObject.SetActive(false);
        }
    }
}