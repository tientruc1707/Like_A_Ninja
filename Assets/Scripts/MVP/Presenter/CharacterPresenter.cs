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

    //Used on Animation 
    public void PerformSkill(int positionSKill)
    {
        _skills[positionSKill - 1].StartPerformingSkill();
    }

    public void UseSkill(int skillPosition)
    {
        Debug.Log(skillPosition);
        if (_mana.CurrentMana >= _skills[skillPosition].skillModel.manaCost)
            _animator.SetBool($"Skill{skillPosition + 1}", true);
        else
            Debug.Log($"Not Enough Mana to use skill{skillPosition + 1}");
    }

    public Sprite SetSkillSprite(int skillPosition)
    {
        return _skills[skillPosition].skillModel.sprite;
    }

    public void UseCharacterStats(bool needUsing)
    {
        GetComponent<ManaPresenter>().enabled = needUsing;
        GetComponent<HealthPresenter>().enabled = needUsing;
    }
    
}
