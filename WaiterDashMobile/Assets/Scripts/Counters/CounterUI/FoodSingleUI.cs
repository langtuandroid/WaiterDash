using Assets.Scripts.Counters.CounterUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI foodName;
    [SerializeField] private TextMeshProUGUI foodPrice;
    [SerializeField] private Image foodIcon;
    private RestaurantObject restaurantObject;

    public void SetRestaurantObjectSO(RestaurantObject restaurantObject)
    {
        this.restaurantObject = restaurantObject;
        foodName.text = restaurantObject.GetRestaurantObjectSO().objectName;
        foodIcon.sprite = restaurantObject.GetRestaurantObjectSO().sprite;
        foodPrice.text = restaurantObject.GetRestaurantObjectSO().price.ToString();
    }

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (restaurantObject != null)
            {
                OrdersOverview.Instance.AddSelectedItem(restaurantObject);
            }
        });
    }
    private void Update()
    {

    }
}
