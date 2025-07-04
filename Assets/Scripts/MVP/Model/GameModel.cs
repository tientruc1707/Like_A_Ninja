
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameModel : MonoBehaviour
{
    public int timeForTurn;
    private float _timeRemaining;
    private bool _isPausingTimer = false;
    [SerializeField] private TextMeshProUGUI timer;

    [Header("Player Stats", order = 1)]
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private Slider _playerManaSlider;
    [SerializeField] private Button[] _playerSkillButtons;

    [Header("Enemy Stats", order = 2)]
    [SerializeField] private Slider _enemyHealthSlider;
    [SerializeField] private Slider _enemyManaSlider;
    [SerializeField] private Button[] _enemySkillButtons;

    private CharacterPresenter _player;
    private CharacterPresenter _enemy;

    private Vector3 _playerPos = new(-1.3f, 1.5f);
    private Vector3 _enemyPos = new(1.3f, 1.5f);

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

    public void InitPlayer(bool flip)
    {
        GameManager.Instance.SetPlayer(SaveSystem.Instance.GetCharacterKey(), _playerPos);
        _player = GameManager.Instance.GetPlayer().GetComponent<CharacterPresenter>();
        _player.ApplyCharacterStatsUI();
        _player.GetComponent<SpriteRenderer>().flipX = flip;
        _player.GetComponent<ManaPresenter>().SetSlider(_playerManaSlider);
        _player.GetComponent<HealthPresenter>().SetSlider(_playerHealthSlider);

        for (int i = 0; i < _playerSkillButtons.Length; i++)
        {
            _playerSkillButtons[i].GetComponent<Image>().sprite = _player.SetSkillSprite(i);
            _playerSkillButtons[i].interactable = true;
            _playerSkillButtons[i].transition = Selectable.Transition.None;
            int skillIndex = i;
            _playerSkillButtons[i].onClick.AddListener(() =>
            {
                _player.UseSkill(skillIndex);
            });
        }
    }

    public void InitEnemy(bool flip)
    {
        GameManager.Instance.SetCurrentEnemy(_enemyPos);
        _enemy = GameManager.Instance.GetCurrentEnemy().GetComponent<CharacterPresenter>();
        _enemy.GetComponent<SpriteRenderer>().flipX = flip;
        _enemy.ApplyCharacterStatsUI();
        _enemy.GetComponent<ManaPresenter>().SetSlider(_enemyManaSlider);
        _enemy.GetComponent<HealthPresenter>().SetSlider(_enemyHealthSlider);
        for (int i = 0; i < _enemySkillButtons.Length; i++)
        {
            _enemySkillButtons[i].GetComponent<Image>().sprite = _enemy.SetSkillSprite(i);
            _enemySkillButtons[i].interactable = false;
            _enemySkillButtons[i].transition = Selectable.Transition.None;
            int skillIndex = i;
            _enemySkillButtons[i].onClick.AddListener(() =>
            {
                _enemy.UseSkill(skillIndex);
            });
        }

    }

    public void SetPlayerSkillButtonsInteractable()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
        {
            foreach (var button in _playerSkillButtons)
            {
                button.interactable = true;
            }
        }
        else
        {
            foreach (var button in _playerSkillButtons)
            {
                button.interactable = false;
            }
        }
    }

}
