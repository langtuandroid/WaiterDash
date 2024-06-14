using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Threading.Tasks;
using System.Linq;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    [SerializeField] private TextMeshProUGUI goldCounter;
    public float goldBalance;
    private static readonly string[] Suffixes = { "", "k", "m", "b", "t", "qd", "qn" }; // Extend this array for more suffixes
    private void Awake()
    {
        Instance = this;
    }

    private void OnApplicationQuit()
    {
        saveGoldBalanceOnCloud();
    }

    private async void Start()
    {
        Application.targetFrameRate = 300;
        await loadMoneyFromCloudAsync();
    }

    public void addGoldBalance(float amountToAdd)
    {
        goldBalance += amountToAdd;
        updateGoldBalanceUI();
    }

    public bool canAffordItem(float amount)
    {
        if (goldBalance >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 

    public void subtractGoldBalance(float amountToSubtract)
    {
        goldBalance -= amountToSubtract;
        updateGoldBalanceUI();
    }

    public async Task loadMoneyFromCloudAsync()
    {
        string currencyID = "GOLD";
        await EconomyService.Instance.Configuration.SyncConfigurationAsync();
        var goldCurrencyDefinition = EconomyService.Instance.Configuration.GetCurrency(currencyID);
        PlayerBalance playerGoldBalance = await goldCurrencyDefinition.GetPlayerBalanceAsync();
        goldBalance = playerGoldBalance.Balance;
        updateGoldBalanceUI();
    }

    public void updateGoldBalanceUI()
    {
        goldCounter.text = FormatBalance(goldBalance);
    }

    // Gold Balance Save methods
    public async void saveGoldBalanceOnCloud()
    {
        await EconomyService.Instance.PlayerBalances.SetBalanceAsync("GOLD", (int)goldBalance);
    }
    
    // Format Gold Balance
    public string FormatBalance(float value)
    {
        if (value < 1000)
        {
            return value.ToString();
        }

        int suffixIndex = 0;
        double reducedValue = value;

        while (reducedValue >= 1000 && suffixIndex < Suffixes.Length - 1)
        {
            reducedValue /= 1000;
            suffixIndex++;
        }

        return reducedValue.ToString("0.#") + Suffixes[suffixIndex];
    }

}
