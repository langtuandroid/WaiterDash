using Assets.Scripts.Customers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningCustomerOrderManager : MonoBehaviour
{
    private List<RestaurantObject> restaurantObjectLists;

    private void Start()
    {
        // Initialize the list
        restaurantObjectLists = new List<RestaurantObject>();
    }

    public void GenerateCustomerOrders()
    {
        ResetOrders();
        var availableFoods = FoodManager.Instance.GetAvailableFoodObjectLists();
        int randomIndex = Random.Range(0, availableFoods.Count);
        RestaurantObject randomFood = availableFoods[randomIndex];
        restaurantObjectLists.Add(randomFood);
    }

    public CustomerOrder GetCustomerOrders()
    {
        return FoodManager.Instance.generateCustomerOrder();
    }

    public void ResetOrders()
    {
        restaurantObjectLists = new List<RestaurantObject>();
    }
}
