
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class TileSelector : MonoBehaviour
{
    public int x, y;
    public List<ItemSO> itemList;
    public Dictionary<(int, int), Sprite> m_ItemDict = new();
    public Image img;
    private ItemSO m_ItemSO;
    public ItemSO ItemType { get => m_ItemSO; private set => m_ItemSO = value; }
    [SerializeField] private Button m_Button;

    private void Awake()
    {
        InitDict();
    }
    void InitDict()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            //0 - normal type
            //1 - special type 1
            //2 - special type 2
            m_ItemDict.Add((i, 0), itemList[i].normalSprite);
            m_ItemDict.Add((i, 1), itemList[i].special_1_Sprite);
            m_ItemDict.Add((i, 2), itemList[i].special_2_Sprite);
        }
    }

    public void SetActiveTile(int CellType, int SpecialType)
    {
        if (CellType == -1)
        {
            img.sprite = null;
        }
        else
        {
            img.sprite = m_ItemDict[(CellType, SpecialType)];
            m_ItemSO = itemList[CellType];
        }
    }

    public void SetPositionInBoard(int row, int col)
    {
        x = row;
        y = col;
    }
}
