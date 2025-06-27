
using TMPro;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    public int timeForTurn;
    private int _timeRemain;
    [SerializeField] private TextMeshProUGUI timer;
    public GameObject leftTurn;
    public GameObject rightTurn;

    public TextMeshProUGUI Timer { private get; set; }
    private void Start()
    {
        _timeRemain = timeForTurn;
        leftTurn.SetActive(true);
        rightTurn.SetActive(false);
        timer.text = _timeRemain.ToString();
    }

    public void UpdateTime()
    {
        _timeRemain -= 1;
        if (_timeRemain <= 0)
        {
            Debug.Log("Time's up!");
            _timeRemain = timeForTurn;
            ChangeSide();
        }

        timer.text = _timeRemain.ToString();

    }
    public void ChangeSide()
    {
        if (leftTurn.activeSelf)
        {
            leftTurn.SetActive(false);
            rightTurn.SetActive(true);
        }
        else
        {
            leftTurn.SetActive(true);
            rightTurn.SetActive(false);
        }
    }
}
