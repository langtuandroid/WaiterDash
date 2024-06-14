using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UpgradeSO;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private Image costImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image maxLevelImage;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;

    private IUpgradeable selectedUpgradeable;
    private IPrefabSelectable currentSelected;
    private bool isMaxLevel = false;


    private void Start()
    {
        panel.SetActive(false);
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        closeButton.onClick.AddListener(Hide);
        BaseUpgradeable.OnUpgradedToNewPrefab += HandleUpgradeToNewPrefab;
    }
    private void HandleUpgradeToNewPrefab(BaseUpgradeable newObject)
    {
        if (currentSelected != null && selectedUpgradeable != null)
        {
            selectedUpgradeable = newObject;
            currentSelected = newObject.GetComponent<IPrefabSelectable>();
            UpdateUI();
        }
    }
    public void Show(IPrefabSelectable selectable, IUpgradeable upgradeable, UpgradeSO upgradeData)
    {
        currentSelected = selectable;
        selectedUpgradeable = upgradeable;
        UpdateUI();
        panel.SetActive(true);
    }

    private void UpdateUI()
    {
        var upgradeData = selectedUpgradeable.GetUpgradeSO();
        var currentLevel = selectedUpgradeable.GetCurrentLevel();
        isMaxLevel = currentLevel == (upgradeData.levels.Count + 1);
        if (isMaxLevel)
        {
            var upgradeLevel = upgradeData.GetUpgradeLevel(currentLevel - 1);
            UpdateTextFields(upgradeData, currentLevel, upgradeLevel);
            UpdateIcon(upgradeData);
        }
        else
        {
            var upgradeLevel = upgradeData.GetUpgradeLevel(currentLevel);
            UpdateTextFields(upgradeData, currentLevel, upgradeLevel);
            UpdateIcon(upgradeData);
        }

        UpdateUpgradeAvailability(upgradeData, currentLevel);
    }

    private void UpdateTextFields(UpgradeSO upgradeData, int currentLevel, UpgradeLevel upgradeLevel)
    {
        nameText.text = upgradeData.Name;
        levelText.text = currentLevel.ToString();
        upgradeDescription.text = upgradeLevel.descriptions.Count > 0 ? upgradeLevel.descriptions[0].description : string.Empty;
        upgradeCost.text = upgradeLevel.upgradeCost.ToString();
    }

    private void UpdateIcon(UpgradeSO upgradeData)
    {
        iconImage.sprite = upgradeData.icon;
    }

    private void UpdateUpgradeAvailability(UpgradeSO upgradeData, int currentLevel)
    {
        upgradeButton.gameObject.SetActive(!isMaxLevel);
        upgradeCost.gameObject.SetActive(!isMaxLevel);
        costImage.gameObject.SetActive(!isMaxLevel);
        if (isMaxLevel)
        {
            nextLevelText.text = "MAX";
            upgradeDescription.text = string.Empty;
        }
        else
        {
            nextLevelText.text = (currentLevel + 1).ToString();
        }
        maxLevelImage.gameObject.SetActive(isMaxLevel);
    }


    public void Hide()
    {
        panel.SetActive(false);
        if (currentSelected != null)
        {
            currentSelected.Deselect();
            currentSelected = null;
            selectedUpgradeable = null;
        }
    }

    private void OnUpgradeButtonClicked()
    {
        if (selectedUpgradeable != null)
        {
            var upgradeCost = selectedUpgradeable.GetUpgradeSO().GetUpgradeLevel(selectedUpgradeable.GetCurrentLevel()).upgradeCost;
            if (CurrencyManager.Instance.canAffordItem(upgradeCost))
            {
                CurrencyManager.Instance.subtractGoldBalance(upgradeCost);
                selectedUpgradeable.Upgrade();
                UpdateUI();
            }

        }
    }
}
