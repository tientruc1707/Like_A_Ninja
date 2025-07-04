using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameModel _gameModel;

    public GameObject leftSide;
    public GameObject rightSide;


    private void OnEnable()
    {
        _gameModel.InitEnemy(true);
        _gameModel.InitPlayer(false);
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.CHANG_SIDE, ChangeSide);
    }

    private void OnDisable()
    {
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.CHANG_SIDE, ChangeSide);
    }

    void Start()
    {
        SetActiveSide();
    }

    private void ChangeSide()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
            GameManager.Instance.CurrentSide = TurnSide.RIGHTTURN;
        else
            GameManager.Instance.CurrentSide = TurnSide.LEFTTURN;

        _gameModel.SetPlayerSkillButtonsInteractable();
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
