using System.Collections.Generic;
using UnityEngine;

public class CharacterPresenter : MonoBehaviour
{
    public CharacterData characterData;
    public string CharacterName => characterData.CharacterName;
    public Sprite CharacterSprite => characterData.CharacterSprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private Mana _mana;
    [SerializeField] private List<SkillPresenter> _skills;

    private void OnEnable()
    {
        GetComponent<HealthPresenter>().enabled = false;
        GetComponent<ManaPresenter>().enabled = false;
        GetComponent<SpriteRenderer>().flipX = false;
    }

    //Used on Animation 
    public void PerformSkill(int positionSKill)
    {
        _skills[positionSKill - 1].StartPerformingSkill();
    }

    public void UseSkill(int skillPosition)
    {
        if (_mana.CurrentMana >= _skills[skillPosition].skillModel.manaCost)
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

}
