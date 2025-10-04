using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDetails : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string Description { get; set; }
    [SerializeField] private GameObject m_skillDetailPanel;
    private TMP_Text m_skillDetailText;
    private float m_holdTime = 0f;
    private bool m_isHolding = false;

    void Start()
    {
        m_skillDetailText = m_skillDetailPanel.GetComponentInChildren<TextMeshProUGUI>(true);
    }
    void Update()
    {
        if (m_isHolding)
        {
            m_holdTime += Time.deltaTime;
            if (m_holdTime >= 0.5f)
            {
                m_skillDetailPanel.SetActive(true);
                m_skillDetailText.text = Description;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_isHolding = true;
        m_holdTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_isHolding = false;
        m_holdTime = 0f;
        m_skillDetailText.text = "";
        m_skillDetailPanel.SetActive(false);
    }
}
