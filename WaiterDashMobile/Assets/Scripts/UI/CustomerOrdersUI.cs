using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrdersUI : MonoBehaviour
{
    [SerializeField] private SingleCustomerOrder OrderTemplate;
    public static CustomerOrdersUI Instance;
    private List<BaseDiningTable> diningTableOrders = new List<BaseDiningTable>();
    private List<SingleCustomerOrder> customerOrdersUI = new List<SingleCustomerOrder>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        OrderTemplate.gameObject.SetActive(false);
    }

    public void AddCustomerOrderUI(List<RestaurantObject> customerOrders)
    {
        var orderedDiningTable = CasaTableUI.Instance.getDiningTable();
        if (diningTableOrders.Contains(orderedDiningTable))
        {
            
        } else
        {
            diningTableOrders.Add(orderedDiningTable);
            SingleCustomerOrder singleCustomerOrder = Instantiate(OrderTemplate, this.transform);
            singleCustomerOrder.gameObject.SetActive(true);
            singleCustomerOrder.setDiningTable(orderedDiningTable);
            singleCustomerOrder.setCustomerOrders(customerOrders);
            customerOrdersUI.Add(singleCustomerOrder);
        }
    }

    public void RemoveCustomerOrderUI(SingleCustomerOrder singleCustomerOrder)
    {
        diningTableOrders.Remove(singleCustomerOrder.getDiningTable());
        customerOrdersUI.Remove(singleCustomerOrder);
    }

    public void RemoveCustomerOrderUI(BaseDiningTable customerDiningTable)
    {
        foreach (var singleCustomerOrder in customerOrdersUI)
        {
            var diningTable = singleCustomerOrder.getDiningTable();
            if(diningTable == customerDiningTable)
            {
                diningTableOrders.Remove(customerDiningTable);
                customerOrdersUI.Remove(singleCustomerOrder);
                singleCustomerOrder.destroySelf();
                return;
            }
        }
    }
}
