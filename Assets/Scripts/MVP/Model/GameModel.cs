
using System;
using TMPro;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    public event Action ChangeSide;
    public int timeForTurn;
    private float _timeRemaining;
    [SerializeField] private TextMeshProUGUI timer;

    private void Start()
    {
        _timeRemaining = timeForTurn;
    }

    void Update()
    {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0)
        {
            ChangeSide?.Invoke();
            _timeRemaining = timeForTurn;
        }
        DisplayTime(_timeRemaining);
    }

    public void DisplayTime(float time)
    {
        timer.text = Mathf.FloorToInt(time).ToString();
    }

}
