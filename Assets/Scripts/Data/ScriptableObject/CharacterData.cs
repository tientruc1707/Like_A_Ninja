using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public string Description;
    public float Health;
    public SkillData[] Skills;
}
