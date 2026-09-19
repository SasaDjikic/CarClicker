using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private UpgradeData upgradeData;
    private Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

    public UnityEvent<string, int> OnUpgradePurchased = new UnityEvent<string, int>();
    public UnityEvent OnMultiplierChanged = new UnityEvent();

    private CurrencySystem _currencySystem;
    private CarController _carController;

    private void Start()
    {
        if (upgradeData == null)
        {
            upgradeData = Resources.Load<UpgradeData>("Config/UpgradeData");
            if (upgradeData == null)
            {
                upgradeData = ScriptableObject.CreateInstance<UpgradeData>();
                upgradeData.Initialize();
            }
        }

        InitializeUpgrades();

        _currencySystem = GameManager.Instance.CurrencySystem;
        _carController = GameManager.Instance.CarController;

        if (_currencySystem != null)
        {
            _currencySystem.SetMultiplier(CalculateMultiplier());
        }
    }

    private void InitializeUpgrades()
    {
        _upgradeLevels.Clear();
        _upgradeLevels["speed"] = 0;
        _upgradeLevels["wheels"] = 0;
        _upgradeLevels["nitro"] = 0;
        _upgradeLevels["spoiler"] = 0;
    }

    public bool PurchaseUpgrade(string upgradeId)
    {
        if (!_upgradeLevels.ContainsKey(upgradeId))
            return false;

        var upgradeInfo = upgradeData.GetUpgrade(upgradeId);
        if (upgradeInfo == null)
            return false;

        int currentLevel = _upgradeLevels[upgradeId];
        if (currentLevel >= upgradeInfo.maxLevel)
            return false;

        int nextLevel = currentLevel + 1;
        int cost = upgradeData.CalculateCost(upgradeId, nextLevel);

        if (_currencySystem.GetPoints() < cost)
            return false;

        _currencySystem.AddPoints(-cost);
        _upgradeLevels[upgradeId]++;

        UpdateMultiplier();
        _carController.AddUpgradeLevel();

        OnUpgradePurchased.Invoke(upgradeId, _upgradeLevels[upgradeId]);

        return true;
    }

    public int GetUpgradeLevel(string upgradeId)
    {
        return _upgradeLevels.ContainsKey(upgradeId) ? _upgradeLevels[upgradeId] : 0;
    }

    public int GetUpgradeCost(string upgradeId)
    {
        if (!_upgradeLevels.ContainsKey(upgradeId))
            return 0;

        int nextLevel = _upgradeLevels[upgradeId] + 1;
        return upgradeData.CalculateCost(upgradeId, nextLevel);
    }

    public bool CanAfford(string upgradeId)
    {
        if (_currencySystem == null)
            return false;

        return _currencySystem.GetPoints() >= GetUpgradeCost(upgradeId);
    }

    public bool IsMaxLevel(string upgradeId)
    {
        var upgradeInfo = upgradeData.GetUpgrade(upgradeId);
        if (upgradeInfo == null)
            return false;

        return GetUpgradeLevel(upgradeId) >= upgradeInfo.maxLevel;
    }

    private void UpdateMultiplier()
    {
        if (_currencySystem != null)
        {
            _currencySystem.SetMultiplier(CalculateMultiplier());
            OnMultiplierChanged.Invoke();
        }
    }

    private float CalculateMultiplier()
    {
        int totalLevels = 0;
        foreach (var kvp in _upgradeLevels)
        {
            totalLevels += kvp.Value;
        }
        return 1f + (totalLevels * 0.1f);
    }

    public int GetTotalUpgradeLevels()
    {
        int total = 0;
        foreach (var kvp in _upgradeLevels)
            total += kvp.Value;
        return total;
    }

    public float GetCurrentMultiplier()
    {
        return CalculateMultiplier();
    }
}
