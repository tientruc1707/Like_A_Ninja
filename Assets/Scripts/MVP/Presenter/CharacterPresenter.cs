using System.Collections.Generic;
using UnityEngine;

public class CharacterPresenter : MonoBehaviour
{
    public CharacterData characterData;
    public string CharacterName => characterData.CharacterName;
    public Sprite CharacterSprite => characterData.CharacterSprite;

    [SerializeField] private List<SkillPresenter> skills;

    //Used on Animation 
    public void PerformSkill(int positionSKill)
    {
        skills[positionSKill - 1].StartPerformingSkill();
    }

    
}
