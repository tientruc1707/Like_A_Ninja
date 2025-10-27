
using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField] private Button _pauseButton;
    private GameObject _board;
    void Awake()
    {
        GameManager.Instance.LoadLevel();
    }

    void OnEnable()
    {
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.WIN_GAME, OnWinGame);
    }

    void OnDisable()
    {
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.WIN_GAME, OnWinGame);
    }

    private void OnWinGame()
    {
        _board.SetActive(false);
        UiManager.Show<VictoryView>();
    }

    public override void Initialize()
    {
        AudioManager.Instance.PlayBackgroundSound(StringConstant.SoundName.BACKGROUND.BATTLE_THEME);
        _pauseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
            _board.SetActive(false);
            UiManager.Show<PausingView>();
        });

    }
    private void Start()
    {
        UiManager.Instance.OnSceneLoaded();
        UiManager.Instance.RegisterStartingView(this);
        _board = GameObject.Find("Board");
    }

}