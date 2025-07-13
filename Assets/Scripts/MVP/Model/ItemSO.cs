using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public int value;
    public Sprite normalSprite;
    public Sprite special_1_Sprite;
    public Sprite special_2_Sprite;

}
