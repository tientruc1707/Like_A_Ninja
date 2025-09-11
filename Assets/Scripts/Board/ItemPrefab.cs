using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemPrefab : MonoBehaviour
{
    public ItemSO itemData;
    private SpriteRenderer m_spriteRenderer;

    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        //        Vector2 spriteSize = m_spriteRenderer.sprite.bounds.size;
        if (itemData != null && m_spriteRenderer != null)
        {
            m_spriteRenderer.sprite = itemData.normalSprite;
        }
        // Vector3 newScale = new Vector3(1 / spriteSize.x, 1 / spriteSize.y, 1);
        // transform.localScale = newScale;
    }
}
