using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField] private Button _pauseButton;

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


    public override void Initialize()
    {
        _pauseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
            UiManager.Show<PausingView>();
        });
    }
    private void OnEnable()
    {
        UiManager.Instance.RegisterStartingView(this);
    }

    public void InitMainCharacter()
    {
        GameManager.Instance.SetMainCharacter(SaveSystem.Instance.GetCharacterKey(), _playerPos);
        _player = GameManager.Instance.GetMainCharacter().GetComponent<CharacterPresenter>();
        _player.UseCharacterStats(true);
        _player.GetComponent<ManaPresenter>().SetSlider(_playerManaSlider);
        _player.GetComponent<HealthPresenter>().SetSlider(_playerHealthSlider);

        for (int i = 0; i < _playerSkillButtons.Length; i++)
        {
            _playerSkillButtons[i].GetComponent<Image>().sprite = _player.SetSkillSprite(i);
            _playerSkillButtons[i].onClick.AddListener(() =>
            {
                _player.UseSkill(i);
            });
        }
    }

    public void InitRandomEnemy()
    {
        _enemy = GameManager.Instance.SetRandomEnemy(_enemyPos).GetComponent<CharacterPresenter>();
        _enemy.GetComponent<ManaPresenter>().SetSlider(_enemyManaSlider);
        _enemy.GetComponent<HealthPresenter>().SetSlider(_enemyHealthSlider);

        for (int i = 0; i < _enemySkillButtons.Length; i++)
        {
            _enemySkillButtons[i].GetComponent<Image>().sprite = _enemy.SetSkillSprite(i);
            _enemySkillButtons[i].onClick.AddListener(() =>
            {
                _enemy.UseSkill(i);
            });
        }
    }
}
