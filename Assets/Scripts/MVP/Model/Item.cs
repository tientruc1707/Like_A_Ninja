using UnityEngine;

public enum ItemType
{
    HEALTH,
    MANA,
    SHURIKEN,
    COIN
}
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public int value;
    public Sprite sprite;
}
