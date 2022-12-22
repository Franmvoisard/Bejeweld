using System.Collections;
using Shoelace.Bejeweld.Views;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    private Vector3 _originalPosition;
    private int _chainCount = 0;
    [SerializeField] private int minChainLengthForShake;
    [SerializeField] private float shakeDuration;
    [SerializeField] private AnimationCurve shakeProfile;
    [SerializeField] private float chainShakeMultiplier;
    
    private static Coroutine _coroutine;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _originalPosition = target.anchoredPosition;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GridView.OnMatchesCleared += OnMatchCleared;
        GridView.OnChainFinished += ResetChainCount;
    }
    private void UnsubscribeToEvents()
    {
        GridView.OnMatchesCleared -= OnMatchCleared;
        GridView.OnChainFinished -= ResetChainCount;
    }

    public void Shake (float duration) {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(CameraShake(duration));
    }

    public IEnumerator CameraShake (float duration) {
        float time = 0;
        float amplitude = 0;
        while (time < duration)
        {
            amplitude = shakeProfile.Evaluate(time);
            time += Time.deltaTime;
            amplitude += _chainCount * chainShakeMultiplier;
            _targetPosition = _originalPosition + Random.insideUnitSphere * amplitude;
            target.anchoredPosition = Vector3.Lerp(_originalPosition,_targetPosition,  time / duration * amplitude);
            yield return null;
        }

        target.anchoredPosition = _originalPosition;
    }
    private void ResetChainCount()
    {
        _chainCount = 0;
    }

    private void OnMatchCleared()
    {
        _chainCount += 1;
        if (_chainCount > minChainLengthForShake)
        {
            Shake(shakeDuration);
        }
    }

    private void OnDestroy() => UnsubscribeToEvents();
}
