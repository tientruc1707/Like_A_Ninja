
using System;
using TMPro;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    public int timeForTurn;
    private float _timeRemaining;
    private bool _isPausingTimer = false;

    [SerializeField] private TextMeshProUGUI timer;

    private void OnEnable()
    {
        _timeRemaining = timeForTurn;
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.CHANG_SIDE, ResetTimer);
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.PAUSE_TIMER, PauseTimer);
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.UNPAUSE_TIMER, UnPauseTimer);
    }

    private void OnDisable()
    {
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.CHANG_SIDE, ResetTimer);
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.PAUSE_TIMER, PauseTimer);
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.UNPAUSE_TIMER, UnPauseTimer);
    }

    void Update()
    {
        if (_isPausingTimer == false)
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0)
            {
                EventSystem.Instance.TriggerEvent(StringConstant.EVENT.CHANG_SIDE);
            }
        }

        DisplayTime(_timeRemaining);
    }

    public void DisplayTime(float time)
    {
        timer.text = Mathf.FloorToInt(time).ToString();
    }

    private void PauseTimer()
    {
        _isPausingTimer = true;
    }

    private void UnPauseTimer()
    {
        _isPausingTimer = false;
    }

    private void ResetTimer()
    {
        _timeRemaining = timeForTurn;
    }

}
