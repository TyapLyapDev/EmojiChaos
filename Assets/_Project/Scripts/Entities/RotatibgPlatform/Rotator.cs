using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    private void Awake() =>
        _rigidbody = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(0, _speed * Time.fixedDeltaTime, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }
}