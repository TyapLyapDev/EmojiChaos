using UnityEngine;

namespace EmojiChaos.Entities.RotatibgPlatform
{
    using Entities.Car;

    public class RotatingPlatform : MonoBehaviour
    {
        private Detector _detector;
        private Transform _parent;

        private void Awake ( )
        {
            _detector = GetComponentInChildren<Detector> ( );
            _parent = GetComponentInChildren<Rotator> ( ).transform;
        }

        private void OnEnable ( )
        {
            _detector.Entered += OnDetectTriggerEnter;
            _detector.Exited += OnDetectTriggerExit;
        }

        private void OnDisable ( )
        {
            _detector.Entered -= OnDetectTriggerEnter;
            _detector.Exited -= OnDetectTriggerExit;
        }

        private void OnDetectTriggerEnter (Collider collider)
        {
            if (collider.TryGetComponent (out Car car))
                car.transform.SetParent (_parent);
        }

        private void OnDetectTriggerExit (Collider collider)
        {
            if (collider.TryGetComponent (out Car car))
            {
                if (car.transform.parent == _parent)
                    car.transform.SetParent (null);
            }
        }
    }
}