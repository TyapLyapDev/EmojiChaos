using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private Detector _detector;
    [SerializeField] private float _speed;

    private Transform _transform;

    private void Awake() =>
        _transform = transform;

    private void OnEnable()
    {
        _detector.Entered += OnDetectTriggerEnter;
        _detector.Exited += OnDetectTriggerExit;
    }

    private void OnDisable()
    {
        _detector.Entered -= OnDetectTriggerEnter;
        _detector.Exited -= OnDetectTriggerExit;
    }

    private void Update() =>
        _transform.rotation *= Quaternion.Euler(0, _speed * Time.deltaTime, 0);

    private void OnDetectTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out Car car))
            car.SetParent(_transform);
    }

    private void OnDetectTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out Car car))
            car.SetParent(null);
    }
}