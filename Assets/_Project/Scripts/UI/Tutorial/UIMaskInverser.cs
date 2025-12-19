using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIMaskInverser : Image
{
    private Material _cachedMaterial;

    public override Material materialForRendering
    {
        get
        {
            if (_cachedMaterial == null)
                _cachedMaterial = new Material(base.materialForRendering);

            Material baseMaterial = base.materialForRendering;

            if (_cachedMaterial.shader != baseMaterial.shader)
                _cachedMaterial.shader = baseMaterial.shader;

            _cachedMaterial.CopyPropertiesFromMaterial(baseMaterial);
            _cachedMaterial.SetInt("_StencilComp", (int)CompareFunction.NotEqual);

            return _cachedMaterial;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (Application.isPlaying)
            Destroy(_cachedMaterial);
        else
            DestroyImmediate(_cachedMaterial);
    }
}