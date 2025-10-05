
using DG.Tweening;
using UnityEngine;


public class SkillPresenter : MonoBehaviour
{
    public Skill skillModel;
    [SerializeField] private Animator _animator;
    [SerializeField] private ManaPresenter _owner;
    private GameObject m_target;
    private Vector3 m_defaultPosition;
    bool flyToTarget = false;

    private void OnEnable()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.RIGHTTURN)
            m_target = GameObject.FindGameObjectWithTag(StringConstant.CHARACTER.PLAYER);
        else
            m_target = GameObject.FindGameObjectWithTag(StringConstant.CHARACTER.ENEMY);
        flyToTarget = false;
        m_defaultPosition = transform.position;
    }

    public void StartPerformingSkill()
    {
        this.gameObject.SetActive(true);
        _owner.DecreaseMana(skillModel.manaCost);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(m_target.tag))
        {
            if (flyToTarget)
                _animator.SetTrigger("Hit");
            CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
            character.TakeDamage(skillModel.damage, GameManager.AnimationState.BIGHURT);
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(m_target.tag)) return;
        CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
        character.EndTakingDamage(GameManager.AnimationState.BIGHURT);
    }

    //added on skill's animation
    public void CompleteExecution(int skillPos)
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.UNPAUSE_TIMER);
        _owner.GetComponent<Animator>().SetBool($"Skill{skillPos}", false);
        transform.DOKill();
        transform.position = m_defaultPosition;
        this.gameObject.SetActive(false);
    }

    //fly to the target position
    public void FlyToTarget()
    {
        transform.DOMove(m_target.transform.position, 1f);
        flyToTarget = true;
    }

}
