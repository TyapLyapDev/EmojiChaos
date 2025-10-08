using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    private const string PropertyName = "_Color";

    [SerializeField] private SpriteRenderer _renderer;

    private MaterialPropertyBlock _propertyBlock;

    public void Initialize() =>
        _propertyBlock = new();

    public void SetColor(Color color)
    {
        if (_renderer == null)
            return;

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(PropertyName, color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}