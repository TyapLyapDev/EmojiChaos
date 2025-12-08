using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static int CalculateLevelCountInProject()
    {
        int levelCount = Resources.LoadAll<Level>(Constants.LevelsPath).Length;
        GC.Collect();
        Resources.UnloadUnusedAssets();

        return levelCount;
    }

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

    public static void CreateDebugSphere(Vector3 position, Color color, string label, float scale)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * scale;

        Renderer renderer = sphere.GetComponent<Renderer>();
        renderer.material.color = color;

        sphere.name = $"DebugSphere_{label}";

        UnityEngine.Object.Destroy(sphere, 2f);
    }

    public static bool IsMobilePlatform()
    {
        return Application.isMobilePlatform ||
               Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
