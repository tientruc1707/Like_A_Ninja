using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    public ItemSO itemData;
    private SpriteRenderer m_spriteRenderer;

    void OnDrawGizmosSelected()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (itemData != null && m_spriteRenderer != null)
        {
            m_spriteRenderer.sprite = itemData.normalSprite;
        }
    }
}
