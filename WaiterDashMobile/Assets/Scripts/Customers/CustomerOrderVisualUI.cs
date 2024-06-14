using Assets.Scripts.Customers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BaseDiningTable;

public class CustomerOrderVisualUI : MonoBehaviour
{
    [SerializeField] private Transform foodTemplate;
    [SerializeField] private Image customerFoodOrderIcon;
    [SerializeField] private Image foodBackground;
    [SerializeField] private Transform drinkTemplate;
    [SerializeField] private Image customerDrinkOrderIcon;
    [SerializeField] private Image drinkBackground;
    private void Start()
    {
        DisableCustomerFoodOrderIcon();
        DisableCustomerDinkOrderIcon();
    }
    public void UpdateCustomerOrderIcon(CustomerOrder customerOrder)
    {
        EnableCustomerFoodOrderIcon();
        customerFoodOrderIcon.sprite = customerOrder.FoodOrder.GetRestaurantObjectSO().sprite;
        if (customerOrder.DrinkOrder)
        {
            EnableCustomerDrinkOrderIcon();
            customerDrinkOrderIcon.sprite = customerOrder.DrinkOrder.GetRestaurantObjectSO().sprite;
        }
    }

    public void DisableCustomerFoodOrderIcon()
    {
        customerFoodOrderIcon.enabled = false;
        foodBackground.enabled = false;
        foodTemplate.gameObject.SetActive(false);
    }

    public void EnableCustomerFoodOrderIcon()
    {
        customerFoodOrderIcon.enabled = true;
        foodBackground.enabled = true;
        foodTemplate.gameObject.SetActive(true);
    }

    public void DisableCustomerDinkOrderIcon()
    {
        customerDrinkOrderIcon.enabled = false;
        drinkBackground.enabled = false;
        drinkTemplate.gameObject.SetActive(false);
    }

    public void EnableCustomerDrinkOrderIcon()
    {
        customerDrinkOrderIcon.enabled = true;
        drinkBackground.enabled = true;
        drinkTemplate.gameObject.SetActive(true);
    }
}
