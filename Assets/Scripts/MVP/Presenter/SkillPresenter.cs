
using UnityEngine;


public class SkillPresenter : MonoBehaviour
{
    public Skill skillModel;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _owner;
    public void StartPerformingSkill()
    {
        this.gameObject.SetActive(true);
        _owner.GetComponent<ManaPresenter>().DecreaseMana(skillModel.manaCost);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(StringConstant.CHARACTER.PLAYER) ||
            collision.CompareTag(StringConstant.CHARACTER.ENEMY))
        {
            CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
            character.TakeDamage(skillModel.damage, StringConstant.AnimationState.BigHurt);
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
        character.EndTakingDamage(StringConstant.AnimationState.BigHurt);
    }

    //added on skill's animation
    public void CompleteExecution(int skillPos)
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.UNPAUSE_TIMER);
        _owner.GetComponent<Animator>().SetBool($"Skill{skillPos}", false);
        this.gameObject.SetActive(false);
    }

}
