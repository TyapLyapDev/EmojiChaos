using UnityEngine;

public class MaterialIndexRepainter : SimpleRepainter
{
    [SerializeField] private int _materialIndex;

    protected override void OnGetPropertyBlock(Renderer renderer, MaterialPropertyBlock propertyBlock) =>
        renderer.GetPropertyBlock(propertyBlock, _materialIndex);

    protected override void OnSetPropertyBlock(Renderer renderer, MaterialPropertyBlock propertyBlock) =>
        renderer.SetPropertyBlock(propertyBlock, _materialIndex);
}