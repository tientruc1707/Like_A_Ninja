
using UnityEngine;


public class SkillPresenter : MonoBehaviour
{
    public Skill skillModel;
    [SerializeField] private Animator animator;
    public void StartPerformingSkill()
    {
        this.gameObject.SetActive(true);
        this.GetComponentInParent<ManaPresenter>().DecreaseMana(skillModel.manaCost);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(StringConstant.CHARACTER.PLAYER))
        {
            HealthPresenter health = collision.GetComponent<HealthPresenter>();
            health.DecreaseHealth(skillModel.damage);
        }
    }

    public void Execute()
    {
        animator.SetTrigger("Execute");
    }

    public void CompleteExecution(int skillPos)
    {
        GetComponentInParent<Animator>().SetBool($"Skill{skillPos - 1}", false);
    }
}
