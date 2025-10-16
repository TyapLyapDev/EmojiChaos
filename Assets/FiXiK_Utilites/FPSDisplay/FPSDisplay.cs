using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    private const int GoodFPS = 60;
    private const int OkFPS = 30;

    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private Button _resetButton;

    [SerializeField] private Color _goodColor = Color.green;
    [SerializeField] private Color _okColor = Color.yellow;
    [SerializeField] private Color _badColor = Color.red;
    [SerializeField] private float _updateInterval = 0.5f;

    [SerializeField] private bool _showMinMax = true;
    [SerializeField] private bool _useColorCoding = true;

    private string _htmlGoodColor;
    private string _htmlOkColor;
    private string _htmlBadColor;

    private float _accumulatedTime = 0f;
    private float _currentFPS = 0f;
    private float _minFPS = float.MaxValue;
    private float _maxFPS = 0f;
    private int _frameCount = 0;

    private void Awake()
    {
        _htmlGoodColor = ColorUtility.ToHtmlStringRGB(_goodColor);
        _htmlOkColor = ColorUtility.ToHtmlStringRGB(_okColor);
        _htmlBadColor = ColorUtility.ToHtmlStringRGB(_badColor);
    }

    private void OnEnable() =>
        _resetButton.onClick.AddListener(ResetMinMax);

    private void OnDisable() =>
        _resetButton.onClick.RemoveListener(ResetMinMax);

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        _accumulatedTime += deltaTime;
        _frameCount++;

        float frameFPS = 1f / deltaTime;
        _minFPS = Mathf.Min(_minFPS, frameFPS);
        _maxFPS = Mathf.Max(_maxFPS, frameFPS);

        if (_accumulatedTime >= _updateInterval)
        {
            _currentFPS = _frameCount / _accumulatedTime;

            if (_fpsText != null)
                _fpsText.text = FormatFPSDisplay();

            _accumulatedTime = 0f;
            _frameCount = 0;
        }
    }

    private void ResetMinMax()
    {
        _minFPS = float.MaxValue;
        _maxFPS = 0f;
    }

    private string FormatFPSDisplay()
    {
        string fpsString = $"FPS: {_currentFPS:0.}";

        if (_showMinMax)
            fpsString += $"\nMin: {_minFPS:0.}\nMax: {_maxFPS:0.}";

        if (_useColorCoding)
        {
            string textColor = _currentFPS >= GoodFPS ? _htmlGoodColor :
                             _currentFPS >= OkFPS ? _htmlOkColor : _htmlBadColor;

            return $"<color=#{textColor}>{fpsString}</color>";
        }

        return fpsString;
    }
}