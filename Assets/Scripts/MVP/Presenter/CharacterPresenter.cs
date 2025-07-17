using System.Collections.Generic;
using UnityEngine;

public class CharacterPresenter : MonoBehaviour
{
    public CharacterData characterData;
    public string CharacterName => characterData.CharacterName;
    public Sprite CharacterSprite => characterData.CharacterSprite;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private HealthPresenter m_Health;
    [SerializeField] private ManaPresenter m_Mana;
    [SerializeField] private List<SkillPresenter> m_SkillList;

    private void OnEnable()
    {
        GetComponent<HealthPresenter>().enabled = false;
        GetComponent<ManaPresenter>().enabled = false;
        GetComponent<SpriteRenderer>().flipX = false;
    }

    //added on Animation 
    public void PerformSkill(int positionSKill)
    {
        m_SkillList[positionSKill - 1].StartPerformingSkill();
    }

    public void UseSkill(int skillPosition)
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);
        m_Animator.SetBool($"Skill{skillPosition + 1}", true);
    }

    public Sprite SetSkillSprite(int skillPosition)
    {
        return m_SkillList[skillPosition].skillModel.sprite;
    }

    public void ApplyCharacterStatsUI(bool needUsing = true)
    {
        GetComponent<ManaPresenter>().enabled = needUsing;
        GetComponent<HealthPresenter>().enabled = needUsing;
    }

    public void Attack()
    {

        m_Animator.SetTrigger(GameManager.AnimationState.Attack);
    }

    public void TakeDamage(float damage, int hurtType)
    {
        m_Health.DecreaseHealth(damage);
        m_Animator.SetBool(hurtType, true);
    }

    public void EndTakingDamage(int hurtType)
    {
        m_Animator.SetBool(hurtType, false);
    }

    public void RestoreHealth(float value)
    {
        m_Health.IncreaseHealth(value);
    }

    public void RestoreMana(float value)
    {
        m_Mana.IncreaseMana(value);
    }

    public bool EnoughManaToUseSkill()
    {
        for (int i = m_SkillList.Count - 1; i >= 0; i--)
        {
            if (m_SkillList[i].skillModel.manaCost <= m_Mana.GetCurrentMana())
            {
                UseSkill(i);
                return true;
            }
        }
        return false;
    }
    
}
