using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PrefabUtility;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private RestaurantObjectSO plateRestaurantObjectSO;
    private float spawnPlateTimer;
    private int platesSpawnedAmount;
    public int platesSpawnedAmountMax = 4;
    public string key = System.Guid.NewGuid().ToString();
    private void Awake()
    {
    }

    private void OnEnable()
    {
        UnityCloudSave.onSavedCloudFileLoaded += OnSavedCloudFileLoaded;
    }

    private void OnDisable()
    {
        UnityCloudSave.onSavedCloudFileLoaded -= OnSavedCloudFileLoaded;
    }

    private void Start()
    {

    }
    private void OnSavedCloudFileLoaded(object sender, EventArgs e)
    {
        LoadState();
    }

    public void SaveState()
    {
        string uniqueID = key + gameObject.name;
        ES3.Save(uniqueID + "_currentLevel", GetCurrentLevel() + 1);
        ES3.Save(uniqueID + "_platesSpawnedAmountMax", platesSpawnedAmountMax);
    }
    private void LoadState()
    {
        string uniqueID = key + gameObject.name;
        if (ES3.KeyExists(uniqueID + "_currentLevel"))
        {
            var level = ES3.Load<int>(uniqueID + "_currentLevel");
            SetCurrentLevel(level);
            UpdateVisuals();
            ResetOutlineDisabled();
        }
        if (ES3.KeyExists(uniqueID + "_platesSpawnedAmountMax"))
        {
            platesSpawnedAmountMax = ES3.Load<int>(uniqueID + "_platesSpawnedAmountMax");
        }

        for (int i = 0; i < platesSpawnedAmountMax; i++)
        {
            platesSpawnedAmount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasRestaurantObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                RestaurantObject.SpawnRestaurantObject(plateRestaurantObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

            }
        }
        else if (player.HasRestaurantObject())
        {
            if (player.GetRestaurantObject().GetRestaurantObjectSO() == plateRestaurantObjectSO)
            {
                if (platesSpawnedAmount < platesSpawnedAmountMax)
                {
                    player.GetRestaurantObject().DestorySelf();
                    platesSpawnedAmount++;
                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
    protected override void ApplyUpgrade(UpgradeSO.UpgradeLevel upgradeLevel)
    {
        foreach (var property in upgradeLevel.properties)
        {
            switch (property.key)
            {
                case "platesSpawnedAmountIncrease":
                    int increaseAmount = (int)property.GetValue();
                    IncreasePlateCount(increaseAmount);
                    break;
                case "upgradeSprite":
                    Sprite upgradeSprite = (Sprite)property.GetValue();
                    // Apply the sprite to a relevant UI element or object
                    break;
                case "upgradePrefab":
                    GameObject upgradePrefab = (GameObject)property.GetValue();
                    // Instantiate or replace the prefab
                    break;
                    // Handle additional properties as needed
            }
        }
        SaveState();
        UnityCloudSave.Instance.saveCloudSavedFile();
    }
    public void IncreasePlateCount(int amount)
    {
        platesSpawnedAmountMax += amount;
        for (int i = 0; i < amount; i++)
        {
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}
