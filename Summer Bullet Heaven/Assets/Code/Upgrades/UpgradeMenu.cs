using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance { get; private set; }

    [SerializeField] private TMPro.TextMeshProUGUI[] upgradeTitles;
    [SerializeField] private TMPro.TextMeshProUGUI[] upgradeDescriptions;
    [SerializeField] private List<ScriptableUpgrade> upgrades = new List<ScriptableUpgrade>();
    private List<ScriptableUpgrade> availableUpgrades = new List<ScriptableUpgrade>();

    private void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void ShowUpgrades()
    {
        gameObject.SetActive(true);
        availableUpgrades.Clear();
        while (availableUpgrades.Count < 3)
        {
            int u = Random.Range(0, upgrades.Count);
            if (!availableUpgrades.Contains(upgrades[u]))
                availableUpgrades.Add(upgrades[u]);
        }
        for (int i = 0; i <3; i++)
        {
            upgradeTitles[i].text = availableUpgrades[i].upgradeName;
            upgradeDescriptions[i].text = availableUpgrades[i].description;
        }
        Time.timeScale = 0.0000001f;
    }

    public void TriggerBuff(int buffIndex)
    {
        if (availableUpgrades[buffIndex].ApplyBuff())
        {
            upgrades.Remove(availableUpgrades[buffIndex]);
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
