using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public Sprite sprite;
    public AudioClip skillSound;
    public string skillName;
    public string description;
    public float manaCost;
    public bool ableToMove;
    public float damage;
}
