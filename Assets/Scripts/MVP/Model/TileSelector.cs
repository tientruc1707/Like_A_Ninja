
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelector : MonoBehaviour
{
    public Sprite emptySprite;
    public LevelSO level;
    public List<ItemSO> itemList;
    public Image img;
    private Dictionary<(int, int), Sprite> itemDict = new();

    void Awake()
    {
        img = GetComponent<Image>();
        itemList = new(level.itemList);

        InitDict();
        SetActiveTile(0, 0);
    }

    void InitDict()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            //0 - normal type
            //1 - special type 1
            //2 - special type 2
            itemDict.Add((i, 0), itemList[i].normalSprite);
            itemDict.Add((i, 1), itemList[i].special_1_Sprite);
            itemDict.Add((i, 2), itemList[i].special_2_Sprite);
        }
    }

    public void SetActiveTile(int typeIndex, int specialIndex)
    {
        if (typeIndex == -1)
        {
            img.sprite = emptySprite;
        }
        else
        {
            img.sprite = itemDict[(typeIndex, specialIndex)];
        }
    }

}
