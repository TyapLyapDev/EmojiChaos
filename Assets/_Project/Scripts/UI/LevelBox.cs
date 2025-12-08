using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelBox : ButtonClickHandler<LevelBox>
{
    [SerializeField] private GameObject _model;
    [SerializeField] private Image _boxImage;
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private StarLevelBox[] _stars;
    [SerializeField] private Sprite _openSprite;
    [SerializeField] private Sprite _closedSprite;

    private UiShaker _shaker;
    private int _number;
    private bool _isLock;

    public int Number => _number;

    public bool IsLock => _isLock;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _shaker?.Dispose();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_isLock == false)
            base.OnPointerEnter(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (IsLock == false)
            base.OnPointerDown(eventData);
    }

    public void SetNumber(int number)
    {
        _number = number;
        _numberText.text = number.ToString();
    }

    public void Lock()
    {
        _isLock = true;

        _boxImage.sprite = _closedSprite;
        _numberText.gameObject.SetActive(false);

        foreach (StarLevelBox star in _stars)
            star.Hide();
    }

    public void Unlock()
    {
        _isLock = false;
        _boxImage.sprite = _openSprite;
        _numberText.gameObject.SetActive(true);

        foreach (StarLevelBox star in _stars)
            star.Show();
    }

    public void SetCountEarnedStars(int value)
    {
        if (value < 0 || value > _stars.Length)
            throw new ArgumentOutOfRangeException($"{nameof(value)} = {value}");

        for (int i = 0; i < _stars.Length; i++)
        {
            if (i < value)
                _stars[i].SetOpenColor();
            else
                _stars[i].SetClosedColor();
        }
    }

    public void ShowCell() =>
        _model.SetActive(true);

    public void HideCell() =>
        _model.SetActive(false);

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _shaker = new(_model.GetComponent<RectTransform>());
    }

    protected override void OnClick()
    {
        if (_isLock == false)
        {
            Audio.Sfx.PlayPointerUpButton();

            return;
        }

        Audio.Sfx.PlayLevelClosed();
        _shaker?.ShakeCombined();
    }
}