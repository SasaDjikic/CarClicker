using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Transform upgradeButtonContainer;
    [SerializeField] private GameObject upgradeButtonPrefab;
    [SerializeField] private UpgradeData upgradeData;
    [SerializeField] private bool autoInitialize = true;

    private UpgradeButton[] _upgradeButtons;

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

        if (autoInitialize)
            InitializeShop();
    }

    public void InitializeShop()
    {
        if (upgradeButtonContainer == null)
        {
            Debug.LogError("Upgrade Button Container not assigned!");
            return;
        }

        _upgradeButtons = new UpgradeButton[upgradeData.upgrades.Length];

        for (int i = 0; i < upgradeData.upgrades.Length; i++)
        {
            GameObject buttonGO;

            if (upgradeButtonPrefab != null)
            {
                buttonGO = Instantiate(upgradeButtonPrefab, upgradeButtonContainer);
            }
            else
            {
                buttonGO = new GameObject($"UpgradeButton_{upgradeData.upgrades[i].upgradeId}");
                buttonGO.transform.parent = upgradeButtonContainer;
                buttonGO.transform.localPosition = Vector3.zero;
                buttonGO.transform.localScale = Vector3.one;

                var layoutElement = buttonGO.AddComponent<LayoutElement>();
                layoutElement.preferredHeight = 80;
                layoutElement.preferredWidth = 150;
            }

            var upgradeButton = buttonGO.GetComponent<UpgradeButton>();
            if (upgradeButton == null)
                upgradeButton = buttonGO.AddComponent<UpgradeButton>();

            upgradeButton.SetUpgradeId(upgradeData.upgrades[i].upgradeId);

            _upgradeButtons[i] = upgradeButton;
        }
    }

    public void RefreshAllButtons()
    {
        foreach (var button in _upgradeButtons)
        {
            if (button != null)
                button.UpdateDisplay();
        }
    }
}
