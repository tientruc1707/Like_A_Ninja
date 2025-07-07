using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    [System.Serializable]
    public class GridPosition
    {
        public ItemSO item;
        public int x;
        public int y;
    }

    public List<ItemSO> itemList;
    public int width;
    public int height;
    public List<GridPosition> gridPositionList;
    public int moveAmount;

}
