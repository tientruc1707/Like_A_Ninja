using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresenter : MonoBehaviour
{
    public CharacterData characterData;
    public string CharacterName => characterData.CharacterName;
    public Sprite CharacterSprite => characterData.CharacterSprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private HealthPresenter _health;
    [SerializeField] private ManaPresenter _mana;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private List<SkillPresenter> _skills;

    private void OnEnable()
    {
        GetComponent<HealthPresenter>().enabled = false;
        GetComponent<ManaPresenter>().enabled = false;
        GetComponent<SpriteRenderer>().flipX = false;
    }

    //added on each skill animation 
    public void PerformSkill(int positionSKill)
    {
        _skills[positionSKill - 1].StartPerformingSkill();
    }

    public void UseSkill(int skillPosition)
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);
        _animator.SetBool($"Skill{skillPosition + 1}", true);
    }

    public Sprite SetSkillSprite(int skillPosition)
    {
        return _skills[skillPosition].skillModel.sprite;
    }

    public void ApplyCharacterStatsUI(bool needUsing = true)
    {
        GetComponent<ManaPresenter>().enabled = needUsing;
        GetComponent<HealthPresenter>().enabled = needUsing;
    }

    public void Attack()
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);
        _animator.SetTrigger(GameManager.AnimationState.ATTACK);
    }

    //Added on normal attack animation
    public void PerformAttack()
    {
        _weapon.ThrowWeapon();
    }

    public void TakeDamage(float damage, int hurtType)
    {
        _health.DecreaseHealth(damage);
        if (_health.GetCurrentHealth() <= 0)
        {
            StartCoroutine(EndGame(1f));
            return;
        }
        else
            _animator.SetBool(hurtType, true);
    }

    public void EndTakingDamage(int hurtType)
    {
        _animator.SetBool(hurtType, false);
    }

    public void RestoreHealth(float value)
    {
        _health.IncreaseHealth(value);
    }

    public void RestoreMana(float value)
    {
        _mana.IncreaseMana(value);
    }

    IEnumerator EndGame(float delay)
    {
        //_animator.SetTrigger("Die");
        yield return new WaitForSeconds(delay);
        if (gameObject.CompareTag("Enemy"))
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.WIN_GAME);
        else if (gameObject.CompareTag("Player"))
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.LOSE_GAME);
    }


}
