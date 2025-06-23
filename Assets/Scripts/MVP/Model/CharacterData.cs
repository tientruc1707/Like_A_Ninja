using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public string Description;
    public Sprite CharacterSprite;
    public float Health;

}
