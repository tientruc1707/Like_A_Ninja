using UnityEngine;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Item Item { get; private set; }

    public Cell NeighbourUp { get; set; }
    public Cell NeighbourDown { get; set; }
    public Cell NeighbourLeft { get; set; }
    public Cell NeighbourRight { get; set; }

    public bool IsEmpty => Item == null;

    public void SetUp(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public bool IsNeighbour(Cell other)
    {
        return X == other.X && Mathf.Abs(Y - other.Y) == 1 ||
               Y == other.Y && Mathf.Abs(X - other.X) == 1;
    }

    public void Free() => Item = null;

    public void SetItem(Item item)
    {
        Item = item;
        item.SetCell(this);
    }

    public void SetItemPosition(bool withAnimationAppear)
    {
        Item.SetViewPosition(this.transform.position);
        if (withAnimationAppear)
            Item.AnimationAppear();
    }

    internal void Clear()
    {
        Item?.CLear();
        Item = null;
    }

    internal bool IsSameType(Cell other)
    {
        return Item != null && other.Item != null && Item.IsSameType(other.Item);
    }

    #region Item Actions
    internal void DestroyItem()
    {
        if(Item == null) return;
        
        Item.AnimationDestroy();
        Item = null;
    }

    internal void AnimationForHint()
    {
        Item.AnimationForHint();
    }

    internal void StopAnimationForHint()
    {
        Item.StopAnimationForHint();
    }

    internal void ApplyItemMoveToPosition()
    {
        Item.AnimationMoveToPosition();
    }
    #endregion

}
