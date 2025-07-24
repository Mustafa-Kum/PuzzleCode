using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HealthTextAssigner : MonoBehaviour

{
    #region Serialized Fields

    [SerializeField] private HealthValuesSO _healthValuesSO;

    [SerializeField] private SampleTimer _sampleTimer;

    [SerializeField] private TextMeshProUGUI _healthText;

    [SerializeField] private TextMeshProUGUI _timerText;

    #endregion

    #region Private Fields

    private const int HealthIncreaseDuration = 45;

    private float remainingTime;

    #endregion

    #region Unity Callbacks

    private void OnEnable()
    {
        EventManager.Resource.InfiniteHealthSetted += SetInfiniteHealth;

        DOVirtual.DelayedCall(0.1f, OnLoaded);
    }

    private void OnDisable()
    {
        EventManager.Resource.InfiniteHealthSetted -= SetInfiniteHealth;
    }

    private void Update()
    {
        if (_healthValuesSO.IsInfiniteHealth) return;
        if (_healthValuesSO.IsHealthFull()) return;

        remainingTime = _sampleTimer.GetUnbiasedRemainingTime();

        UpdateTimerText(remainingTime);

        if (remainingTime <= 0f) IncreaseHealth();
    }

    #endregion

    #region Private Methods

    private void OnLoaded()
    {
        if (_healthValuesSO.IsHealthFull()) return;
        var healthIncrease = _sampleTimer.CalculateHealthIncrease(HealthIncreaseDuration);
        Debug.Log("Health Increase: " + healthIncrease);
        _healthValuesSO.IncreaseHealthValue(healthIncrease);
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        _healthText.SetText(_healthValuesSO.GetHealthValue().ToString());

        if (_healthValuesSO.IsInfiniteHealth)
        {
            _timerText.SetText("INF");

            _healthText.SetText("∞");
        }

        else if (_healthValuesSO.IsHealthFull())
        {
            _timerText.SetText("MAX");
        }

        else
        {
            remainingTime = _sampleTimer.GetUnbiasedRemainingTime();
            if (remainingTime <= 0f && _healthValuesSO.IsHealthFull() == false)
                _sampleTimer.SetUnbiasedTimer(HealthIncreaseDuration);

            EventManager.Resource.HealthChanged?.Invoke();

            UpdateTimerText(remainingTime);
        }
    }

    private void UpdateTimerText(float _remainingTime)
    {
        var minutes = Mathf.FloorToInt(_remainingTime / 60f);

        var seconds = Mathf.FloorToInt(_remainingTime % 60f);

        _timerText.SetText($"{minutes:00}:{seconds:00}");
    }

    private void IncreaseHealth()
    {
        _healthValuesSO.IncreaseHealthValue(1);

        UpdateUI();
    }

    private void SetInfiniteHealth()
    {
        _healthText.SetText("∞");

        _timerText.SetText("INF");

        _healthValuesSO.SetHealthToMax();

        EventManager.Resource.HealthChanged?.Invoke();
    }

    #endregion
}