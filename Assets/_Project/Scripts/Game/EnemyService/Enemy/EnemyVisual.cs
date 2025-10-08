using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private const string BackgroundColorName = "_BackgroundColor";

    [SerializeField] private Renderer _renderer;

    private MaterialPropertyBlock _propertyBlock;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_renderer == null)
            throw new System.Exception($"{nameof(_renderer)} не назначен");

        _propertyBlock = new();
        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        if(_isInitialized == false)
            throw new System.Exception("Попытка назначить дцет до инициализации");

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(BackgroundColorName, color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}