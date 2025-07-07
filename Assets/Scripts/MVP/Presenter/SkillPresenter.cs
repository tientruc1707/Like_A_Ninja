
using UnityEngine;


public class SkillPresenter : MonoBehaviour
{
    public SkillSO skillModel;
    [SerializeField] private Animator _animator;
    [SerializeField] private ManaPresenter _owner;
    public void StartPerformingSkill()
    {
        this.gameObject.SetActive(true);
        _owner.DecreaseMana(skillModel.manaCost);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(StringConstant.CHARACTER.PLAYER) ||
            collision.CompareTag(StringConstant.CHARACTER.ENEMY))
        {
            CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
            character.TakeDamage(skillModel.damage, GameManager.AnimationState.BigHurt);
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
        character.EndTakingDamage(GameManager.AnimationState.BigHurt);
    }

    //added on skill's animation
    public void CompleteExecution(int skillPos)
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.UNPAUSE_TIMER);
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.CHANG_SIDE);
        _owner.GetComponent<Animator>().SetBool($"Skill{skillPos}", false);
        this.gameObject.SetActive(false);
    }

}
