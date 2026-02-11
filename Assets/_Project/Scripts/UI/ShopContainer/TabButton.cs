using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ButtonText _buttonText;
        [SerializeField] private Image _image;
        [SerializeField] private Color _selected;
        [SerializeField] private Color _unselected;

        private TabPanel _tabPanel;

        public event Action<TabButton> Clicked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnClick);

        public void Init(TabPanel tabPanel, LanguageTextsSet buttonText)
        {
            _buttonText.SetText(buttonText);
            _tabPanel = tabPanel;
        }

        public void SetSelectStatus(bool isSelected)
        {
            _image.color = isSelected ? _selected : _unselected;

            if (isSelected)
                _tabPanel.Show();
            else
                _tabPanel.Hide();
        }

        private void OnClick() =>
            Clicked?.Invoke(this);
    }
}