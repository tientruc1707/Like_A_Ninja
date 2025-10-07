using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private List<AudioClip> _audioSources;
    [SerializeField] private AudioSource _audioSource;
    private void OnEnable()
    {
        GetComponent<HealthPresenter>().enabled = false;
        GetComponent<ManaPresenter>().enabled = false;
        GetComponent<SpriteRenderer>().flipX = false;
        foreach (var skill in _skills)
        {
            _audioSources.Add(skill.skillModel.skillSound);
        }
    }

    //added on each skill animation 
    public void PerformSkill(int positionSKill)
    {
        _skills[positionSKill - 1].StartPerformingSkill();
    }

    public void UseSkill(int skillPosition)
    {
        if (!(GameManager.Instance.CurrentSide == TurnSide.RIGHTTURN) && CompareTag("Enemy") &&
            !(GameManager.Instance.CurrentSide == TurnSide.LEFTTURN) && CompareTag("Player"))
        {
            return;
        }

        if (_mana.GetCurrentMana() < _skills[skillPosition].skillModel.manaCost)
        {
            if (CompareTag(StringConstant.CHARACTER.PLAYER))
                EventSystem.Instance.TriggerEvent(StringConstant.EVENT.MANA_ISSUE);
            return;
        }
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);
        _audioSource.PlayOneShot(_audioSources[skillPosition], 0.5f);
        _animator.SetBool($"Skill{skillPosition + 1}", true);
    }

    public Sprite SetSkillSprite(int skillPosition)
    {
        return _skills[skillPosition].skillModel.sprite;
    }

    public string GetSkillDescription(int skillPosition)
    {
        return _skills[skillPosition].skillModel.description;
    }

    public float GetSkillManaCost(int skillPosition)
    {
        return _skills[skillPosition].skillModel.manaCost;
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
        AudioManager.Instance.PlaySFX(StringConstant.SoundName.SFX.ATTACK);
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

    //Added on skill's animation to throw the skill object to the target if skill has to throw object
    public void ThrowSkillObject(int skillPos)
    {
        _skills[skillPos - 1].FlyToTarget();
    }

}
