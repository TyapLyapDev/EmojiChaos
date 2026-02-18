using System.Collections;
using UnityEngine;

public class SportCarMovementParticle : MonoBehaviour
{
    [SerializeField] private SimpleRepainter[] _repainters;
    [SerializeField] private Car _car;

    private void Awake()
    {
        foreach (var repainter in _repainters)
            repainter.Initialize();
    }

    private IEnumerator Start()
    {
        yield return null;

        SetColor(_car.Color);
    }

    private void SetColor(Color color)
    {
        foreach (var repainter in _repainters)
            repainter.SetColor(color);
    }
}