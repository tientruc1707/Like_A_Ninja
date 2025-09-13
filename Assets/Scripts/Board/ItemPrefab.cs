using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemPrefab : MonoBehaviour
{
    public ItemSO itemData;
    private SpriteRenderer m_spriteRenderer;
    private CharacterPresenter currentCharacter;


    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (itemData != null && m_spriteRenderer != null)
        {
            m_spriteRenderer.sprite = itemData.sprite;
        }

    }

    private void OnEnable()
    {
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.CHANG_SIDE, ChangeCharacter);
    }

    void Start()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
        {
            currentCharacter = GameManager.Instance.GetPlayer().GetComponent<CharacterPresenter>();
        }
        else
        {
            currentCharacter = GameManager.Instance.GetCurrentEnemy().GetComponent<CharacterPresenter>();
        }
    }

    void OnDestroy()
    {
        if (itemData.itemType == ItemType.OTHER) return;
        switch (itemData.itemType)
        {
            case ItemType.HEALTH:
                currentCharacter.RestoreHealth(itemData.value);
                break;
            case ItemType.MANA:
                currentCharacter.RestoreMana(itemData.value);
                break;
            case ItemType.SHURIKEN:
                currentCharacter.Attack();
                break;
            default:
                break;
        }

        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.CHANG_SIDE, ChangeCharacter);
    }

    private void ChangeCharacter()
    {
        if (currentCharacter == null) return;
        if (currentCharacter.CompareTag("Enemy"))
        {
            currentCharacter = GameManager.Instance.GetPlayer().GetComponent<CharacterPresenter>();
        }
        else
        {
            currentCharacter = GameManager.Instance.GetCurrentEnemy().GetComponent<CharacterPresenter>();
        }
    }

}
