using UnityEngine;

[System.Serializable]
public class UpgradeInfo
{
    public string upgradeId;
    public string displayName;
    public string description;
    public int baseCost = 100;
    public float costMultiplier = 1.15f;
    public int maxLevel = 50;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "CarClicker/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public UpgradeInfo[] upgrades = new UpgradeInfo[4];

    public void Initialize()
    {
        if (upgrades.Length == 0)
        {
            upgrades = new UpgradeInfo[4];
        }

        upgrades[0] = new UpgradeInfo
        {
            upgradeId = "speed",
            displayName = "Speed",
            description = "+0.1 km/h",
            baseCost = 100,
            costMultiplier = 1.15f,
            maxLevel = 50
        };

        upgrades[1] = new UpgradeInfo
        {
            upgradeId = "wheels",
            displayName = "Wheels",
            description = "+0.1 km/h",
            baseCost = 150,
            costMultiplier = 1.15f,
            maxLevel = 50
        };

        upgrades[2] = new UpgradeInfo
        {
            upgradeId = "nitro",
            displayName = "Nitro",
            description = "+0.1 km/h",
            baseCost = 200,
            costMultiplier = 1.15f,
            maxLevel = 50
        };

        upgrades[3] = new UpgradeInfo
        {
            upgradeId = "spoiler",
            displayName = "Spoiler",
            description = "+0.1 km/h",
            baseCost = 120,
            costMultiplier = 1.15f,
            maxLevel = 50
        };
    }

    public UpgradeInfo GetUpgrade(string upgradeId)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeId == upgradeId)
                return upgrade;
        }
        return null;
    }

    public int CalculateCost(string upgradeId, int level)
    {
        var upgrade = GetUpgrade(upgradeId);
        if (upgrade == null) return 0;

        return Mathf.RoundToInt(upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, level - 1));
    }
}
