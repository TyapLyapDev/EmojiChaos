using UnityEngine;

public class CircleMask : MonoBehaviour
{
    [SerializeField] private Color _color = Color.magenta;
    [SerializeField] private int _segments = 32;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _color;
        Vector3 scale = transform.lossyScale;
        float radius = Mathf.Max(scale.x, scale.z) * 0.5f;

        float angle = 0f;
        Vector3 lastPoint = Vector3.zero;

        for (int i = 0; i <= _segments; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            Vector3 point = transform.position + new Vector3(x * radius, 0, z * radius);

            if (i > 0)
                Gizmos.DrawLine(lastPoint, point);

            lastPoint = point;
            angle += 2 * Mathf.PI / _segments;
        }
    }
}