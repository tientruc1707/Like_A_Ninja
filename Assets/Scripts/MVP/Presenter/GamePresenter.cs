using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameModel _gameModel;
    [SerializeField] private GameView _gameView;
    public GameObject leftSide;
    public GameObject rightSide;


    private void OnEnable()
    {
        _gameModel.ChangeSide += ChangeSide;
    }

    private void OnDisable()
    {
        _gameModel.ChangeSide -= ChangeSide;
    }

    void Start()
    {
        SetActiveSide();
        _gameView.InitEnemy(true);
        _gameView.InitPlayer(false);
    }

    private void ChangeSide()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
            GameManager.Instance.CurrentSide = TurnSide.RIGHTTURN;
        else
            GameManager.Instance.CurrentSide = TurnSide.LEFTTURN;
        _gameView.SetPlayerSkillButtonsInteractable();
        SetActiveSide();
    }

    private void SetActiveSide()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
        {
            leftSide.SetActive(true);
            rightSide.SetActive(false);
        }
        else
        {
            leftSide.SetActive(false);
            rightSide.SetActive(true);
        }
    }


}
