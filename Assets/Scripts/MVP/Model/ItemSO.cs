using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public int value;
    public Sprite sprite;
}
