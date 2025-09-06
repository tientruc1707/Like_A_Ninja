using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class Item
{
    public Cell Cell { get; private set; }
    public Transform View { get; private set; }

    public virtual void SetView()
    {
        string prefabName = GetPrefabName();
        if (!string.IsNullOrEmpty(prefabName))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab)
            {
                View = GameObject.Instantiate(prefab).transform;
            }
        }
    }

    protected virtual string GetPrefabName()
    {
        return string.Empty;
    }

    public string GetItemTypeName()
    {
        return this.GetPrefabName();
    }
    #region Properties
    internal void SetCell(Cell cell) { Cell = cell; }

    public void SetViewPosition(Vector3 pos)
    {
        if (View)
        {
            View.position = pos;
        }
    }

    public void SetViewRoot(Transform root)
    {
        if (View)
        {
            View.SetParent(root);
        }
    }

    public void SetSortingLayerHigher()
    {
        if (View)
        {
            SpriteRenderer spriteRenderer = View.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sortingOrder = 1;
            }
        }
    }

    public void SetSortingLayerLower()
    {
        if (View)
        {
            SpriteRenderer spriteRenderer = View.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sortingOrder = 0;
            }
        }
    }
    #endregion

    #region Animations

    internal void AnimationMoveToPosition()
    {
        if (View == null || Cell == null) return;
        View.DOMove(Cell.transform.position, 0.2f).SetEase(Ease.InOutSine);
    }

    internal void AnimationAppear()
    {
        if (View == null) return;
        View.localScale = Vector3.zero;
        View.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    internal virtual void AnimationDestroy()
    {
        if (View)
        {
            View.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GameObject.Destroy(View.gameObject);
                View = null;
            });
        }
    }

    internal void AnimationForHint()
    {
        if (View)
        {
            View.DOPunchScale(Vector3.one * 0.2f, 0.1f).SetLoops(-1);
        }
    }

    internal void StopAnimationForHint()
    {
        if (View)
        {
            View.DOKill();
        }
    }
    #endregion

    internal virtual bool IsSameType(Item other)
    {
        return false;
    }

    internal void CLear()
    {
        Cell = null;
        if (View)
        {
            GameObject.Destroy(View.gameObject);
            View = null;
        }
    }

}
