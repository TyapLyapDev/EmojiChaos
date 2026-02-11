using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Detector : MonoBehaviour
{
    private SphereCollider _sphereCollider;

    public event Action<Collider> Entered;
    public event Action<Collider> Exited;

    private void Awake() =>
        _sphereCollider = GetComponent<SphereCollider>();

    private void OnTriggerEnter(Collider other) =>
        Entered?.Invoke(other);

    private void OnTriggerExit(Collider other) =>
        Exited?.Invoke(other);

    public void PerformByForce()
    {
        Vector3 center = transform.TransformPoint(_sphereCollider.center);
        
        float radius = _sphereCollider.radius * Mathf.Max(
            transform.lossyScale.x,
            transform.lossyScale.y,
            transform.lossyScale.z
        );

        Collider[] colliders = new Collider[20];
        int count = Physics.OverlapSphereNonAlloc(center, radius, colliders);

        for (int i = 0; i < count; i++)
            if (colliders[i] != _sphereCollider)
                Entered?.Invoke(colliders[i]);
    }
}