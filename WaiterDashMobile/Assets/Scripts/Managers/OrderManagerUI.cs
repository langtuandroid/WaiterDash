using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManagerUI : MonoBehaviour
{
    [SerializeField] private Transform orderContainer;
    [SerializeField] private Transform orderTemplate;
    [SerializeField] private List<Transform> OrderTemplateLists;
    public static OrderManagerUI Instance { get; private set; }
    private void Awake()
    {
        orderTemplate.gameObject.SetActive(false);
        orderContainer.gameObject.SetActive(true);
    }

    private void Update()
    {

    }
    private void Start()
    {
        Instance = this;
        KitchenManager.OnOrderReceived += OrderReceived;
        KitchenManager.OnOrderCompleted += OrderCompleted;
        foreach (Transform child in orderContainer)
        {
            if (child == orderTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void OrderCompleted(object sender, EventArgs e)
    {
        RemoveFirstOrderTemplate();
    }

    private void OrderReceived(object sender, EventArgs e)
    {
        var restaurantObject = sender as RestaurantObject;
        Transform restaurantObjectTransform = Instantiate(orderTemplate, orderContainer);
        OrderTemplateLists.Add(restaurantObjectTransform);
        restaurantObjectTransform.gameObject.SetActive(true);
        restaurantObjectTransform.GetComponent<OrderSingleUI>().SetOrderedFood(restaurantObject, restaurantObjectTransform);
    }

    public Transform GetFirstOrderTemplate()
    {
        if (OrderTemplateLists.Count <= 0)
        {
            return null;
        }
        else
        {
            return OrderTemplateLists[0];
        }
    }

    public void RemoveFirstOrderTemplate()
    {
        Transform restaurantObjectTransform = OrderTemplateLists[0];
        OrderTemplateLists.RemoveAt(0);
        Destroy(restaurantObjectTransform.gameObject);
    }
}
