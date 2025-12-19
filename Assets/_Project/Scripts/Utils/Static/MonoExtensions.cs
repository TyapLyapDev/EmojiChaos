using System;
using UnityEngine;

public static class MonoExtensions
{
    public static bool IsActiveSelf(this MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == null)
            throw new ArgumentNullException(nameof(monoBehaviour));

        return monoBehaviour.gameObject.activeSelf;
    }

    public static void SetPositionAndRotation(this MonoBehaviour monoBehaviour, Vector3 position, Quaternion rotation)
    {
        if (monoBehaviour == null)
            throw new ArgumentNullException(nameof(monoBehaviour));

        monoBehaviour.transform.SetPositionAndRotation(position, rotation);
    }

    public static void SetActive(this MonoBehaviour monoBehaviour, bool isActive)
    {
        if (monoBehaviour == null)
            throw new ArgumentNullException(nameof(monoBehaviour));

        monoBehaviour.gameObject.SetActive(isActive);
    }

    public static Vector3 GetPosition(this MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == null)
            throw new ArgumentNullException(nameof(monoBehaviour));

        return monoBehaviour.transform.position;
    }

    public static void SetParent(this MonoBehaviour monoBehaviour, Transform parent)
    {
        if (monoBehaviour == null)
            throw new ArgumentNullException(nameof(monoBehaviour));

        monoBehaviour.transform.SetParent(parent);
    }
}