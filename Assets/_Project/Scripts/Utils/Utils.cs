using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void Shuffle<T>(List<T> list)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));

        if (list.Count == 0)
            return;

        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T element = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = element;
        }
    }

    public static Vector3 ConvertScreenSwipeToWorldDirection(Vector2 deltaNormalaized, Transform camera)
    {
        Vector3 cameraForward = camera.forward;
        Vector3 cameraRight = camera.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 worldDirection = (cameraForward * deltaNormalaized.y + cameraRight * deltaNormalaized.x).normalized;

        return worldDirection;
    }

    public static int CalculateSwipeDirectionRelativeToCar(Vector3 worldDirection, Transform car)
    {
        Vector3 carForward = car.forward;
        carForward.y = 0f;
        carForward.Normalize();
        float dotProduct = Vector3.Dot(worldDirection, carForward);

        if (Mathf.Abs(dotProduct) > 0.5f)
            return (int)Mathf.Sign(dotProduct);

        return 0;
    }
}