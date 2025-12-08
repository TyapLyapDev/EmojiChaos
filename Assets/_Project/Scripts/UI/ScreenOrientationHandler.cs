using UnityEngine;

public class ScreenOrientationHandler : MonoBehaviour
{
    [SerializeField] private GameObject _horizontalBackground;
    [SerializeField] private GameObject _verticalBackground;

    private Vector2Int _lastResolution;

    void Start()
    {
        _lastResolution = new Vector2Int(Screen.width, Screen.height);
        UpdateOrientationState();

#if UNITY_ANDROID || UNITY_IOS
        Application.onBeforeRender += HandleResolutionChanged;
#endif
    }

    void OnDestroy()
    {
#if UNITY_ANDROID || UNITY_IOS
        Application.onBeforeRender -= HandleResolutionChanged;
#endif
    }

    private void Update() =>
        HandleResolutionChanged();

    private void HandleResolutionChanged()
    {
        Vector2Int currentResolution = new(Screen.width, Screen.height);

        if (_lastResolution != currentResolution)
        {
            _lastResolution = currentResolution;
            UpdateOrientationState();
        }
    }

    private void UpdateOrientationState()
    {
        bool isHorizontal = Screen.width > Screen.height;

        if (_horizontalBackground != null)
            _horizontalBackground.SetActive(isHorizontal);

        if (_verticalBackground != null)
            _verticalBackground.SetActive(isHorizontal == false);
    }
}
