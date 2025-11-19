#if UNITY_EDITOR
using UnityEngine;

public static class DebugUtils
{
    public static void DrawSphere(Vector3 center, float radius, Color color, float duration)
    {
        int segments = 12;
        float angleIncrement = 360f / segments;

        for (int i = 0; i < 3; i++)
        {
            Vector3 axis = i == 0 ? Vector3.up : (i == 1 ? Vector3.right : Vector3.forward);
            Quaternion rotation = Quaternion.LookRotation(axis);
            Vector3 prevPoint = center + rotation * Vector3.forward * radius;

            for (int j = 1; j <= segments; j++)
            {
                float angle = j * angleIncrement;
                Vector3 point = center + rotation * (Quaternion.Euler(0, angle, 0) * Vector3.forward * radius);
                Debug.DrawLine(prevPoint, point, color, duration);
                prevPoint = point;
            }
        }
    }

    public static void DrawSphere(Vector3 center, float radius, Color color)
    {
        int segments = 12;
        float angleIncrement = 360f / segments;

        for (int i = 0; i < 3; i++)
        {
            Vector3 axis = i == 0 ? Vector3.up : (i == 1 ? Vector3.right : Vector3.forward);
            Quaternion rotation = Quaternion.LookRotation(axis);
            Vector3 prevPoint = center + rotation * Vector3.forward * radius;

            for (int j = 1; j <= segments; j++)
            {
                float angle = j * angleIncrement;
                Vector3 point = center + rotation * (Quaternion.Euler(0, angle, 0) * Vector3.forward * radius);
                Debug.DrawLine(prevPoint, point, color);
                prevPoint = point;
            }
        }
    }
}
#endif