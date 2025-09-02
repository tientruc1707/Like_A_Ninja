using UnityEngine;

public class NormalItem : Item
{
    public enum eNormalType
    {
        HEALTH_POTION,
        MANA_POTION,
        SWORD,
        SHIELD
    }

    public eNormalType NormalType { get; private set; }
    public void SetType(eNormalType type) { NormalType = type; }

    protected override string GetPrefabName()
    {
        string prefabName = string.Empty;

        switch (NormalType)
        {
            case eNormalType.HEALTH_POTION:
                prefabName = StringConstant.ITEM_PREFAB_PATH.HEALTH_POTION;
                break;
            case eNormalType.MANA_POTION:
                prefabName = StringConstant.ITEM_PREFAB_PATH.MANA_POTION;
                break;
            case eNormalType.SWORD:
                prefabName = StringConstant.ITEM_PREFAB_PATH.SWORD;
                break;
            case eNormalType.SHIELD:
                prefabName = StringConstant.ITEM_PREFAB_PATH.SHIELD;
                break;
        }

        return prefabName;
    }

    internal override bool IsSameType(Item other)
    {
        NormalItem item = other as NormalItem;
        if (item == null) return false;
        return item.NormalType == NormalType;
    }
}
