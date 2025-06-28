using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public Sprite sprite;
    public string skillName;
    public string desciption;
    public float manaCost;
    public bool ableToMove;
    public float damage;
}
