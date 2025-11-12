using UniRx;
using UnityEngine;

public class IntervalRunner
{
    private readonly System.Action _onIntervalElapsed;
    private System.IDisposable _updateSubscription;
    private float _interval;
    private float _accumulatedTime;
    private bool _isRunning;

    public IntervalRunner(System.Action onIntervalElapsed)
    {
        _onIntervalElapsed = onIntervalElapsed ?? throw new System.ArgumentNullException(nameof(onIntervalElapsed));
    }

    public void StartRunning(float intervalInSeconds)
    {
        if (intervalInSeconds <= 0f)
            throw new System.ArgumentOutOfRangeException(nameof(intervalInSeconds), "Значение должно быть больше нуля");

        if (_isRunning)
            throw new System.InvalidOperationException($"{nameof(StartRunning)}: попытка повторного запуска");

        _isRunning = true;
        _interval = intervalInSeconds;
        _accumulatedTime = 0f;

        _updateSubscription = Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (_isRunning == false)
                    return;

                _accumulatedTime += Time.deltaTime;

                if (_accumulatedTime >= _interval)
                {
                    _accumulatedTime = 0f;
                    _onIntervalElapsed?.Invoke();
                }
            });
    }

    public void StopRunning()
    {
        _isRunning = false;
        _updateSubscription?.Dispose();
        _updateSubscription = null;
    }

    public void Dispose() =>
        StopRunning();
}