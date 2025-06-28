using System.Collections;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    public GameModel gameModel;

    void Start()
    {
        gameModel.leftTurn.SetActive(true);
        gameModel.rightTurn.SetActive(false);

    }

    void Update()
    {
        //StartCoroutine(DecreaseTime());
    }

    IEnumerator DecreaseTime()
    {
        yield return new WaitForSecondsRealtime(1);

        gameModel.UpdateTime();
    }
}
