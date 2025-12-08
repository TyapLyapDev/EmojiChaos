using System;
using UnityEngine;

public class AlphaMaskTextureReplacer : BaseRepainter
{
    [SerializeField] private string _texturePropertyName = "_MainTex";

    private int _textureShaderId;
    private Texture2D _texture;

    public void SetTexture(Texture2D texture)
    {
        ValidateInit(nameof(SetTexture));

        _texture = texture;
        Repaint();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        if (string.IsNullOrEmpty(_texturePropertyName))
            throw new InvalidOperationException(nameof(_texturePropertyName));

        _textureShaderId = Shader.PropertyToID(_texturePropertyName);
    }

    protected override void OnRepaint(MaterialPropertyBlock propertyBlock) =>
        propertyBlock.SetTexture(_textureShaderId, _texture);
}