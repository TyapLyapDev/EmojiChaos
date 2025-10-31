using System;
using UnityEngine;

public static class MonoExtensions
{
    public static Vector3 GetPosition(this MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == null)
            throw new ArgumentNullException(nameof(monoBehaviour));

        return monoBehaviour.transform.position;
    }
}