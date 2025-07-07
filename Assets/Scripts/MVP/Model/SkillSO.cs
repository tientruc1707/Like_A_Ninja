using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class SkillSO : ScriptableObject
{
    public Sprite sprite;
    public string skillName;
    public string desciption;
    public float manaCost;
    public bool ableToMove;
    public float damage;
}
