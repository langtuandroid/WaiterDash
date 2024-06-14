using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KitchenSinkCounter : BaseCounter
{
    public static KitchenSinkCounter Instance;
    public static event EventHandler OnWashingDishes;
    [SerializeField] private RestaurantObjectSO dirtyPlateSO;
    [SerializeField] private RestaurantObjectSO cleanPlateSO;
    [SerializeField] Transform cleanPlateSpawnPosition;
    private bool washingDirtyPlate = false;
    private float dishWashingTime = 5;
    private float dishWashingStartTime = 5;
    public string key = System.Guid.NewGuid().ToString();
    private RestaurantObject dirtyDishObject;

    private void OnEnable()
    {
        UnityCloudSave.onSavedCloudFileLoaded += OnSavedCloudFileLoaded;
    }

    private void OnDisable()
    {
        UnityCloudSave.onSavedCloudFileLoaded -= OnSavedCloudFileLoaded;
    }

    private void awake()
    {
        Instance = this;
    }
    private void OnSavedCloudFileLoaded(object sender, EventArgs e)
    {
        LoadState();
    }

    public override void Interact(Player player)
    {
        if (player.HasRestaurantObject())
        {
            RestaurantObjectSO restaurantObjectSO = player.GetRestaurantObject().GetRestaurantObjectSO();
            if (restaurantObjectSO == dirtyPlateSO)
            {
                dirtyDishObject = player.GetRestaurantObject();
                player.GetRestaurantObject().SetRestaurantObjectParent(this, GetRestaurantObjectFollowTransform());

                washingDirtyPlate = true;
                dishWashingStartTime = dishWashingTime;

            }
        }
        else if (washingDirtyPlate)
        {
            Debug.Log("Continue washing");
        }
        else
        {
            SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
        }
    }
    private void Update()
    {
        if (washingDirtyPlate)
        {
            if (Player.Instance.getSelectedCounter() == this)
            {
                if (Player.Instance.isPlayerInteracting())
                {
                    OnWashingDishes?.Invoke(GetWashingTimerNormalized(), EventArgs.Empty);
                    dishWashingStartTime -= Time.deltaTime;
                    if (dishWashingStartTime < 0f)
                    {
                        Debug.Log("Washed dishes");
                        dirtyDishObject.DestorySelf();
                        washingDirtyPlate = false;
                        RestaurantObject.SpawnRestaurantObject(cleanPlateSO, Player.Instance);
                    }
                }
            }

        }
    }

    public float GetWashingTimerNormalized()
    {
        if (washingDirtyPlate)
        {
            return 1 - (dishWashingStartTime / dishWashingTime);

        }
        else
        {
            return 0;
        }
    }

    protected override void ApplyUpgrade(UpgradeSO.UpgradeLevel upgradeLevel)
    {
        foreach (var property in upgradeLevel.properties)
        {
            switch (property.key)
            {
                case "dishWashingTime":
                    int dirtyDishWashingTime = (int)property.GetValue();
                    dishWashingTime = dirtyDishWashingTime;
                    break;
                case "upgradeSprite":
                    Sprite upgradeSprite = (Sprite)property.GetValue();
                    // Apply the sprite to a relevant UI element or object
                    break;
                case "upgradePrefab":
                    GameObject upgradePrefab = (GameObject)property.GetValue();
                    // Instantiate or replace the prefab
                    break;
            }
        }
        SaveState();
        UnityCloudSave.Instance.saveCloudSavedFile();
    }

    public void SaveState()
    {
        string uniqueID = key + "_" + gameObject.name;
        ES3.Save(uniqueID + "_currentLevel", GetCurrentLevel() + 1); // +1 because current level is updates after save state
        ES3.Save(uniqueID + "_dishWashingTime", dishWashingTime);
    }

    private void LoadState()
    {
        string uniqueID = key + "_" + gameObject.name;
        if (ES3.KeyExists(uniqueID + "_currentLevel"))
        {
            var level = ES3.Load<int>(uniqueID + "_currentLevel");
            SetCurrentLevel(level);
            UpdateVisuals();
            ResetOutlineDisabled();
        }
        if (ES3.KeyExists(uniqueID + "_dishWashingTime"))
        {
            dishWashingTime = ES3.Load<float>(uniqueID + "_dishWashingTime");
        }
    }
}

