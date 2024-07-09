using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradableStat", menuName = "ScriptableObjects/Stat upgrade")]
public class ScriptableUpgrade : ScriptableObject
{
    public string upgradeName;
    [Multiline] public string description;
    [SerializeField] private UpgradeableStat upgradeableStat;
    [SerializeField] private float upgradeAmmount;

    public void ApplyBuff()
    {
        PlayerControl.CurrentPlayer.UpgradeStat(upgradeableStat, upgradeAmmount);
    }
}

public enum UpgradeableStat
{
    AttackSpeed,
    CurrenHealth,
    Damage,
    DashCooldown,
    DashDistance,
    Pierce,
    ProjectileSpeed,
    Speed
}