using Assets.Scripts.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OrderNoteCustomerOrderVisualUI : MonoBehaviour
{
    [SerializeField] private Transform foodTemplate;
    [SerializeField] private Image customerFoodOrderIcon;
    [SerializeField] private Image foodBackground;
    [SerializeField] private Transform drinkTemplate;
    [SerializeField] private Image customerDrinkOrderIcon;
    [SerializeField] private Image drinkBackground;
    private CustomerOrder customerOrder;
    private BaseDiningTable diningTable;
    public static OrderNoteCustomerOrderVisualUI Instance { get; set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }
    public void UpdateCustomerOrderIcon(CustomerOrder customerOrder, BaseDiningTable diningTable)
    {
        this.diningTable = diningTable;
        this.customerOrder = customerOrder;
        EnableCustomerFoodOrderIcon();
        customerFoodOrderIcon.sprite = customerOrder.FoodOrder.GetRestaurantObjectSO().sprite;
        if (customerOrder.DrinkOrder)
        {
            EnableCustomerDrinkOrderIcon();
            customerDrinkOrderIcon.sprite = customerOrder.DrinkOrder.GetRestaurantObjectSO().sprite;
        }
        else
        {
            DisableCustomerDinkOrderIcon();
        }
    }

    public CustomerOrder GetCustomerOrder()
    {
        return this.customerOrder;
    }

    public BaseDiningTable GetCustomerDiningTable()
    {
        return this.diningTable;
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
