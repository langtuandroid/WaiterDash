using Assets.Scripts.Customers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private List<RestaurantObject> FoodObjectLists;
    [SerializeField] private List<RestaurantObject> DrinkObjectLists;
    public static FoodManager Instance;

    private void Start()
    {
        Instance = this;
    }


    public List<RestaurantObject> GetAvailableFoodObjectLists()
    {
        return FoodObjectLists;
    }
    public List<RestaurantObject> GetAvailableDrinkObjectLists()
    {
        return DrinkObjectLists;
    }

    public CustomerOrder generateCustomerOrder()
    {
        CustomerOrder customerOrder = new CustomerOrder();
        var foodIndex = Random.Range(0, GetAvailableFoodObjectLists().Count);
        customerOrder.FoodOrder = GetAvailableFoodObjectLists()[foodIndex];
        // Determine whether to set a random drink or leave it as null
        if (Random.value < 0.9f) // Adjust the probability as needed
        {
            var drinkIndex = Random.Range(0, GetAvailableDrinkObjectLists().Count);
            customerOrder.DrinkOrder = GetAvailableDrinkObjectLists()[drinkIndex];
        }
        else
        {
            customerOrder.DrinkOrder = null;
        }
        return customerOrder;
    }
}
