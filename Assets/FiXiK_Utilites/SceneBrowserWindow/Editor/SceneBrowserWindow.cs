using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace FiXiK.SceneBrowserWindow.Editor
{
    public class SceneBrowserWindow : EditorWindow
    {
        private const string MenuName = "Tools/Список сцен";
        private const string WindowName = "Список сцен";
        private const string FolderName = "Assets";
        private const string SceneType = "t:Scene";
        private const string HiddenScenesKey = "SceneBrowser_HiddenScenes";
        private const string Tittle = "Избранное:";

        private const string SceneIconName = "SceneAsset Icon";
        private const string FolderIconName = "d_Folder Icon";
        private const string OpenEyeIconName = "d_scenevis_hidden";
        private const string ClosedEyeIconName = "d_scenevis_visible";

        private const string FolderTextureTooltip = "Показать в проекте";
        private const string OpenEyeTextureTooltip = "Скрыть из избранного";
        private const string ClosedEyeTextureTooltip = "Перенести в избранное";
        private const string HiddenSceneBlockName = "Остальные сцены";

        private const int LabelHeight = 25;

        private Texture2D _sceneTexture;
        private Texture2D _folderTexture;
        private Texture2D _openEyeTexture;
        private Texture2D _closedEyeTexture;

        private List<string> _hiddenScenes = new();
        private Vector2 _scrollPosition;
        private string[] _scenePaths;
        private bool _isShowHiddenScenes = false;

        [MenuItem(MenuName)]
        public static void ShowWindow() =>
            GetWindow<SceneBrowserWindow>(WindowName);

        private void OnEnable()
        {
            EditorApplication.projectChanged += RefreshSceneList;

            LoadIcons();
            LoadHiddenScenes();
            RefreshSceneList();
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= RefreshSceneList;
            SaveHiddenScenes();
        }

        private void OnGUI()
        {
            GUILayout.Label(Tittle, EditorStyles.boldLabel);

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            DrawVisibleScenes();

            _isShowHiddenScenes = EditorGUILayout.Foldout(_isShowHiddenScenes, HiddenSceneBlockName, true);

            if (_isShowHiddenScenes && _hiddenScenes.Count > 0)
                DrawHiddenScenes();

            GUILayout.EndScrollView();
        }

        private void DrawVisibleScenes()
        {
            foreach (string scenePath in _scenePaths)
                if (_hiddenScenes.Contains(scenePath) == false)
                    DrawSceneLine(scenePath, _openEyeTexture, OpenEyeTextureTooltip, true);
        }

        private void DrawHiddenScenes()
        {
            foreach (string scenePath in _hiddenScenes.Where(SceneExists).ToList())
                DrawSceneLine(scenePath, _closedEyeTexture, ClosedEyeTextureTooltip, false);
        }

        private void LoadIcons()
        {
            _sceneTexture = EditorGUIUtility.IconContent(SceneIconName).image as Texture2D;
            _folderTexture = EditorGUIUtility.IconContent(FolderIconName).image as Texture2D;
            _openEyeTexture = EditorGUIUtility.IconContent(OpenEyeIconName).image as Texture2D;
            _closedEyeTexture = EditorGUIUtility.IconContent(ClosedEyeIconName).image as Texture2D;
        }

        private void DrawSceneLine(string scenePath, Texture2D actionIcon, string tooltip, bool isVisible = true)
        {
            if (SceneExists(scenePath) == false)
                return;

            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            string folderTooltipWithPath = $"{FolderTextureTooltip}\n{scenePath}";

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(_sceneTexture), GUILayout.Width(LabelHeight), GUILayout.Height(LabelHeight));

            if (GUILayout.Button(new GUIContent("", _folderTexture, folderTooltipWithPath), GUILayout.Width(LabelHeight), GUILayout.Height(LabelHeight)))
                ShowSceneInProject(scenePath);

            if (GUILayout.Button(sceneName, GUILayout.Height(LabelHeight)))
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(scenePath);

            if (GUILayout.Button(new GUIContent("", actionIcon, tooltip), GUILayout.Width(LabelHeight), GUILayout.Height(LabelHeight)))
                ToggleSceneVisibility(scenePath, isVisible);

            GUILayout.EndHorizontal();
        }

        private void ToggleSceneVisibility(string scenePath, bool isVisible)
        {
            if (isVisible)
                HideScene(scenePath);
            else
                UnhideScene(scenePath);
        }

        private void ShowSceneInProject(string scenePath)
        {
            Object sceneAsset = AssetDatabase.LoadAssetAtPath<Object>(scenePath);

            if (sceneAsset != null)
                EditorGUIUtility.PingObject(sceneAsset);
        }

        private void LoadHiddenScenes()
        {
            _hiddenScenes.Clear();
            string hiddenScenesData = EditorPrefs.GetString(HiddenScenesKey, "");

            if (string.IsNullOrEmpty(hiddenScenesData) == false)
            {
                _hiddenScenes = hiddenScenesData.Split(';')
                    .Where(s => string.IsNullOrEmpty(s) == false)
                    .Where(SceneExists)
                    .ToList();

                SaveHiddenScenes();
            }
        }

        private bool SceneExists(string scenePath) =>
            File.Exists(scenePath) && AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null;

        private void SaveHiddenScenes() =>
            EditorPrefs.SetString(HiddenScenesKey, string.Join(";", _hiddenScenes));

        private void RefreshSceneList()
        {
            string[] searchFolders = new[] { FolderName };

            _scenePaths = AssetDatabase.FindAssets(SceneType, searchFolders)
                                      .Select(AssetDatabase.GUIDToAssetPath)
                                      .Where(path => path.EndsWith(".unity") && _hiddenScenes.Contains(path) == false)
                                      .OrderBy(path => Path.GetFileNameWithoutExtension(path))
                                      .ToArray();
        }

        private void HideScene(string scenePath)
        {
            if (_hiddenScenes.Contains(scenePath) == false)
            {
                _hiddenScenes.Add(scenePath);
                _hiddenScenes = _hiddenScenes.OrderBy(path => Path.GetFileNameWithoutExtension(path)).ToList();
                SaveHiddenScenes();
                RefreshSceneList();
            }
        }

        private void UnhideScene(string scenePath)
        {
            if (_hiddenScenes.Contains(scenePath))
            {
                _hiddenScenes.Remove(scenePath);
                SaveHiddenScenes();
                RefreshSceneList();
            }
        }
    }
}