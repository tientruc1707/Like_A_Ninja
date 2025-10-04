using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_manaIssueLog;

    private List<string> m_playerSkillLogs;
    private List<string> m_enemySkillLogs;

    void Start()
    {
        m_playerSkillLogs = new List<string>();
        m_enemySkillLogs = new List<string>();
        GameObject player = GameObject.FindWithTag("Player");
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if (player == null || enemy == null)
        {
            Debug.LogError("Player or Enemy not found in the scene.");
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            string description = player.GetComponent<CharacterPresenter>().GetSkillDescription(i);
            float manaCost = player.GetComponent<CharacterPresenter>().GetSkillManaCost(i);
            string fullText = $"Player used {description} /n Mana Cost: {manaCost}";
            m_playerSkillLogs.Add(fullText);
        }
        for (int i = 0; i < 3; i++)
        {
            string description = enemy.GetComponent<CharacterPresenter>().GetSkillDescription(i);
            float manaCost = enemy.GetComponent<CharacterPresenter>().GetSkillManaCost(i);
            string fullText = $"Enemy used {description} /n Mana Cost: {manaCost}";
            m_enemySkillLogs.Add(fullText);
        }
    }

    private void OnEnable()
    {
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.MANA_ISSUE, ShowManaIssueLog);
    }

    private void OnDisable()
    {
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.MANA_ISSUE, ShowManaIssueLog);
    }

    public void ShowManaIssueLog()
    {
        m_manaIssueLog.SetActive(true);
        Invoke(nameof(HideManaIssueLog), 1.5f);
    }

    private void HideManaIssueLog()
    {
        m_manaIssueLog.SetActive(false);
    }

}
