using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseCounter : BaseUpgradeable, IInteractable, IRestaurantObjectParent, IPlacementObject, IUpgradeable, IPrefabSelectable, IPointerClickHandler
{
    public static event EventHandler OnAnyObjectPlacedHere;
    public static event Action<IPrefabSelectable> OnSelectableSelected;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform prefabVisual;
    [SerializeField] private GameObject visualsParent;
    public Vector2Int prefabSize = new Vector2Int(1,1);
    private RestaurantObject restaurantObject;

    private void Start()
    {
        
    }
    public virtual void Interact(Player player)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!RestaurantManager.Instance.IsRestaurantOpen() && !CameraTouchMovementManager.Instance.isDragging)
        {
            OnSelectableSelected?.Invoke(this);
        }
    }

    protected override void ApplyUpgrade(UpgradeSO.UpgradeLevel upgradeLevel)
    {
        // Handle common upgrade logic for all counters, if any
    }

    public new virtual void Upgrade()
    {
        base.Upgrade();
        UpdateVisuals();
        ResetOutlineEnabled();
    }

    protected void UpdateVisuals()
    {
        int currentLevel = GetCurrentLevel();
        Transform lastVisual = null;
        
        // Loop through all child GameObjects
        foreach (Transform visual in visualsParent.transform)
        {
            int level = GetLevelFromGameObject(visual.gameObject);
            if(level <= currentLevel)
            {
                visual.gameObject.SetActive(true);
                lastVisual = visual;
            }
        }

        foreach (Transform visual in visualsParent.transform)
        {
            if(visual != lastVisual)
            {
                visual.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator AddOutlineNextFrame()
    {
        yield return null; // Wait until the next frame
        gameObject.AddComponent<Outline>();
    }

    private int GetLevelFromGameObject(GameObject obj)
    {
        // Get the name of the GameObject
        string name = obj.name;

        // Remove any non-numeric characters from the name
        string levelString = Regex.Replace(name, "[^0-9]", "");

        // Parse the remaining string as an integer
        int level;
        if (int.TryParse(levelString, out level))
        {
            // Return the parsed level
            return level;
        }
        else
        {
            // If parsing fails, return a default value or handle the error as needed
            return 0;
        }
    }

    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetRestaurantObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetRestaurantObject(RestaurantObject restaurantObject)
    {
        this.restaurantObject = restaurantObject;
        if(restaurantObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public RestaurantObject GetRestaurantObject()
    {
        return restaurantObject;
    }

    public void ClearRestaurantObject()
    {
        restaurantObject = null;
    }

    public bool HasRestaurantObject()
    {
        return restaurantObject != null;
    }

    public Transform GetPrefab()
    {
        return this.transform;
    }

    public Transform GetPrefabVisual()
    {
        return prefabVisual;
    }

    public Vector2Int GetPrefabSize()
    {
        return prefabSize;
    }
}
