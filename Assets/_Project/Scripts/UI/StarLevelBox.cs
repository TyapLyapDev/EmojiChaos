using UnityEngine;
using UnityEngine.UI;

public class StarLevelBox : MonoBehaviour
{
    [SerializeField] private Image _star;
    [SerializeField] private Color _openColor;
    [SerializeField] private Color _closedColor;

    public void SetOpenColor() =>
        _star.color = _openColor;

    public void SetClosedColor() => 
        _star.color = _closedColor;

    public void Show() =>
        gameObject.SetActive(true);

    public void Hide() =>
        gameObject.SetActive(false);
}