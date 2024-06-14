using Assets.Scripts.Counters.FridgeCounter_UI;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeCounterUI : ListViewController<StoreView, RestaurantObjectSO>
{
    public static FridgeCounterUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        itemTemplate.gameObject.SetActive(false);
        items = ItemListManager.Instance.GetFridgeProducts();
        PopulateItemList();
        FridgeCounter.Instance.OnFridgeCounterInteract += OnFridgeCounterInteract;
    }

    private void OnFridgeCounterInteract(object sender, EventArgs e)
    {
        
    }

    protected override void InstantiateUIElements(Transform fridgeProduct, RestaurantObjectSO itemData)
    {
        fridgeProduct.GetComponent<FridgeProductSingleUI>().setProduct(itemData);
    }

    public  void closeFridgeCounterUI()
    {
        this.gameObject.SetActive(false);
    }
}
