using System.Collections.Generic;
using UnityEngine;

public class LevelsPanel : PanelBase 
{
    [SerializeField] private LevelBox _levelBoxPrefab;
    [SerializeField] private Transform _context;

    private readonly List<LevelBox> _levelBoxes = new();
    private readonly SceneLoader _sceneLoader = new();
    private readonly Saver _saver = new();
    private int _levelCount;

    public void Initialize()
    {
        ClearContext();
        _levelCount = Resources.LoadAll<Level>(Constants.LevelsPath).Length;
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        for (int i = 0; i < _levelCount; i++)
        {
            LevelBox levelBox = Instantiate(_levelBoxPrefab, _context);
            levelBox.Initialize(i + 1);
            levelBox.Clicked += OnLevelClick;
            _levelBoxes.Add(levelBox);
        }
    }

    protected override void OnShow()
    {
       
    }

    private void ClearContext()
    {
        foreach (Transform child in _context)
            Destroy(child.gameObject);

        _levelBoxes.Clear();
    }

    private void OnLevelClick(LevelBox levelBbox)
    {
        _saver.SaveCurrentLevel(levelBbox.Number - 1);
        _sceneLoader.LoadScene(Constants.LevelSceneName);
    }
}