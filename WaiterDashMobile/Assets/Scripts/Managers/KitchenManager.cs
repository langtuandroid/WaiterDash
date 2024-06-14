using Assets.Scripts.UI;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public static KitchenManager Instance;
    public static EventHandler OnOrderReceived;
    public static EventHandler OnOrderCompleted;
    private List<RestaurantObject> orderLists = new List<RestaurantObject>();
    private List<ItemSingleUI> itemSingleUIList = new List<ItemSingleUI>();
    private bool isCooking;
    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        if(orderLists.Count > 0 && !KitchenFoodCounter.Instance.HasRestaurantObject())
        {
            var firstInQueueOrder = orderLists[0];
            var foodCookingTime = firstInQueueOrder.GetRestaurantObjectSO().cookingTime;
            if (!isCooking)
            {
                isCooking = true;
                FunctionTimer.Create(() =>
                {
                    isCooking = false;
                    KitchenFoodCounter.Instance.SpawnOrderedFood(firstInQueueOrder);
                    orderLists.RemoveAt(0);
                    if (GetFirstInQueueItemSingleUI().IsOrderItemCompleted())
                    {
                        itemSingleUIList.RemoveAt(0);
                    }
                    OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                }, foodCookingTime);
                
            }
        }
    }

    public void CookOrderedFood(List<RestaurantObject> orders, List<ItemSingleUI> orderItemSingle)
    {
        itemSingleUIList.AddRange(orderItemSingle);
        foreach (var order in orders)
        {
            RestaurantObjectSO orderSO = order.GetRestaurantObjectSO();
            if(orderSO.objectType != ObjectType.Drinks)
            {
                for (var i = 0; orderSO.orderQuantity > i; i++)
                {
                    orderLists.Add(order);
                    OnOrderReceived?.Invoke(order, EventArgs.Empty);
                }
            }
        }
    }

    public List<RestaurantObject> KitchenFoodOrders()
    {
        return orderLists;
    }

    public RestaurantObject GetFirstInQueueRestaurantObject()
    {
        return orderLists[0];
    }

    public ItemSingleUI GetFirstInQueueItemSingleUI()
    {
        if (itemSingleUIList.Count <= 0)
        {
            return null;
        }
        else
        {
            return itemSingleUIList[0];
        }
    }

    public bool HasItemSingleUI()
    {
        return itemSingleUIList.Count > 0;
    }
}
