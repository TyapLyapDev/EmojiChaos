using UnityEngine;
using UnityEngine.Splines;


[ExecuteInEditMode]
public class InstallerOnSpline : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField, Min(0)] private int _splineIndex;
    [SerializeField, Range(0, 1)] private float _progress;
    [SerializeField] private Vector3 _additionalRotation = Vector3.zero;
    [SerializeField] private bool _isSplineDirection = true;

    [Header("Компоненты")]
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private SplineRendererMeshGenerator _splineRendererMeshGenerator;
    [SerializeField] private bool _isAutoupdate = true;


    private Spline _currentSpline;
    private bool _isSubscribedToEvents = false;

    private void OnValidate()
    {
        if (IsValid() == false)
            FindComponents();

        if (_isAutoupdate)
            UpdateTransform();
    }

    private void Reset() =>
        FindComponents();

    private void OnEnable() =>
        SubscribeToEvents();

    private void OnDisable() =>
        UnsubscribeFromEvents();

    private bool IsValid()
    {
        bool isValid = true;

        if (_splineContainer == null)
            isValid = false;

        if (_currentSpline == null)
            isValid = false;

        if (_splineRendererMeshGenerator == null)
            isValid = false;

        return isValid;
    }

    private void FindComponents()
    {
        _splineContainer = null;
        _splineRendererMeshGenerator = null;
        _currentSpline = null;

        _splineContainer = GetComponentInParent<SplineContainer>(true);

        if (_splineContainer == null)
        {
            Debug.LogWarning($"Не удалось получить компонент {nameof(_splineContainer)}");

            return;
        }

        _splineRendererMeshGenerator = _splineContainer.GetComponent<SplineRendererMeshGenerator>();

        if (_splineRendererMeshGenerator == null)
        {
            Debug.LogWarning($"Не удалось получить компонент {nameof(_splineRendererMeshGenerator)}");

            return;
        }

        if (_splineIndex >= 0 && _splineIndex < _splineContainer.Splines.Count)
            _currentSpline = _splineContainer.Splines[_splineIndex];

        if(_currentSpline == null)
        {
            Debug.LogWarning($"Не удалось получить компонент {nameof(_currentSpline)}");

            return;
        }
    }

    private void SubscribeToEvents()
    {
        if (IsValid() == false)
            return;

        if (_isSubscribedToEvents)
            return;

        _splineRendererMeshGenerator.Changed += OnMeshGeneratorChanged;
        _isSubscribedToEvents = true;
    }

    private void UnsubscribeFromEvents()
    {
        if (_isSubscribedToEvents == false)
            return;

        if(_splineRendererMeshGenerator != null)
            _splineRendererMeshGenerator.Changed -= OnMeshGeneratorChanged;

        _isSubscribedToEvents = false;
    }

    private void OnMeshGeneratorChanged()
    {
        if (IsValid() && _isAutoupdate)
            UpdateTransform();
    }

    [ContextMenu("Обновить позицию и поворот")]
    public void UpdateTransform()
    {
        if (IsValid() == false)
        {
            Debug.LogWarning("Невозможно обновить, отсутствуют ссылки на необходимые компоненты");

            return;
        }

        if (TryCalculatePosition(out Vector3 position) == false)
            return;

        if (TryCalculateRotation(out Quaternion rotation) == false)
            return;

        if (position != null && rotation != null)
            transform.SetPositionAndRotation(position, rotation);

        transform.localScale = _splineRendererMeshGenerator.ScaleAdjustment;

        if (TryGetComponent(out Star star))
            star.SetProgress(_progress);
    }

    private bool TryCalculatePosition(out Vector3 position)
    {
        try
        {
            position = (Vector3)_splineContainer.EvaluatePosition(_splineIndex, _progress) + _splineRendererMeshGenerator.PositionAdjustment;

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка при оценке положения сплайна: {e.Message}", this);
            position = Vector3.zero;

            return false;
        }
    }

    private bool TryCalculateRotation(out Quaternion rotation)
    {
        try
        {
            if(_isSplineDirection == false)
            {
                rotation = transform.rotation;

                return true;
            }

            Vector3 localTangent = _splineContainer.EvaluateTangent(_splineIndex, _progress);
            Vector3 worldTangent = _splineContainer.transform.TransformDirection(localTangent);

            if (worldTangent == Vector3.zero)
            {
                rotation = transform.rotation;

                return true;
            }

            Quaternion baseRotation = Quaternion.LookRotation(worldTangent);
            rotation = baseRotation * Quaternion.Euler(_additionalRotation);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error evaluating spline rotation: {e.Message}", this);
            rotation = Quaternion.identity;

            return false;
        }
    }

#endif
}