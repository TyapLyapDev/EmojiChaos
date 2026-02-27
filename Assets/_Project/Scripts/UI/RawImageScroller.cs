using UnityEngine;
using UnityEngine.UI;

public class RawImageScroller : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _horizontalSpeed;

    private void Update() =>
        Scroll();

    private void Scroll()
    {
        Rect rect = _rawImage.uvRect;
        rect.x += _horizontalSpeed * Time.deltaTime;
        rect.y += _verticalSpeed * Time.deltaTime;
        _rawImage.uvRect = rect;
    }
}