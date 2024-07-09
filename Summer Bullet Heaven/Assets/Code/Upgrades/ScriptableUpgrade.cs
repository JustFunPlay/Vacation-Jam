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

    public virtual bool ApplyBuff()
    {
        PlayerControl.CurrentPlayer.UpgradeStat(upgradeableStat, upgradeAmmount);
        return false;
    }
}

public enum UpgradeableStat
{
    AttackSpeed,
    CurrentHealth,
    Damage,
    DashCooldown,
    DashDistance,
    Pierce,
    ProjectileSpeed,
    Speed,
    Other
}