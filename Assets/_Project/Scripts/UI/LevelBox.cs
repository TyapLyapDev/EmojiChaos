using TMPro;
using UnityEngine;

public class LevelBox : ButtonClickHandler<LevelBox>
{
    [SerializeField] private TextMeshProUGUI _numberText;

    private int _number;

    public int Number => _number;

    public void Initialize(int number)
    {
        _number = number;
        _numberText.text = number.ToString();
    }
}
