using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private string upgradeId;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image upgradeIcon;

    private UpgradeSystem _upgradeSystem;
    private UpgradeData _upgradeData;

    private void Start()
    {
        _upgradeSystem = GameManager.Instance.GetComponent<UpgradeSystem>();
        _upgradeData = Resources.Load<UpgradeData>("Config/UpgradeData");

        if (_upgradeData == null)
        {
            _upgradeData = ScriptableObject.CreateInstance<UpgradeData>();
            _upgradeData.Initialize();
        }

        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyClick);

        _upgradeSystem.OnUpgradePurchased.AddListener(UpdateDisplay);
        _upgradeSystem.OnMultiplierChanged.AddListener(UpdateDisplay);

        UpdateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void OnBuyClick()
    {
        _upgradeSystem.PurchaseUpgrade(upgradeId);
    }

    public void UpdateDisplay()
    {
        var upgradeInfo = _upgradeData.GetUpgrade(upgradeId);
        if (upgradeInfo == null)
            return;

        int level = _upgradeSystem.GetUpgradeLevel(upgradeId);
        int cost = _upgradeSystem.GetUpgradeCost(upgradeId);
        bool canAfford = _upgradeSystem.CanAfford(upgradeId);
        bool isMaxLevel = _upgradeSystem.IsMaxLevel(upgradeId);

        if (upgradeNameText != null)
            upgradeNameText.text = upgradeInfo.displayName;

        if (levelText != null)
            levelText.text = $"Level: {level}";

        if (costText != null)
        {
            if (isMaxLevel)
                costText.text = "MAX";
            else
                costText.text = $"Cost: {cost}";
        }

        if (buyButton != null)
        {
            buyButton.interactable = canAfford && !isMaxLevel;
        }

        if (upgradeIcon != null && upgradeInfo.icon != null)
            upgradeIcon.sprite = upgradeInfo.icon;
    }

    public void SetUpgradeId(string id)
    {
        upgradeId = id;
    }
}
