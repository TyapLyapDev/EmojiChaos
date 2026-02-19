using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/AnimationStrategy")]
public class UiAnimation : ScriptableObject
{
    [Header ("Start")]
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Quaternion _startRotation;
    [SerializeField] private float _startAlpha;

    [Header ("Target")]
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private Quaternion _targetRotation;
    [SerializeField] private float _targetAlpha;

    [Space (10)]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private Ease _ease = Ease.OutElastic;

    public Vector3 StartScale => _startScale;

    public Quaternion StartRotation => _startRotation;

    public float StartAlpha => _startAlpha;

    public Vector3 TargetScale => _targetScale;

    public Quaternion TargetRotation => _targetRotation;

    public float TargetAlpha => _targetAlpha;

    public float Duration => _duration;

    public Ease Ease => _ease;
}