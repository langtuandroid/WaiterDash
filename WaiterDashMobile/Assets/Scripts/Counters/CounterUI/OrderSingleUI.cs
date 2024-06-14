using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private Image timerImage;
    private float cookingTime;
    private float cookingTimeStartTime;
    private bool isCooking;
    private RestaurantObject restaurantObject;
    private Transform orderTemplate;

    private void Start()
    {
        timerImage.fillAmount = 0;
    }
    public void SetOrderedFood(RestaurantObject restaurantObject, Transform orderTemplate)
    {
        orderText.text = restaurantObject.GetRestaurantObjectSO().name.ToString();
        this.restaurantObject = restaurantObject;
        this.orderTemplate = orderTemplate;
        cookingTime = this.restaurantObject.GetRestaurantObjectSO().cookingTime;
        cookingTimeStartTime = this.restaurantObject.GetRestaurantObjectSO().cookingTime;
    }

    private void Update()
    {
        if(!KitchenFoodCounter.Instance.HasRestaurantObject() && restaurantObject != null && orderTemplate == OrderManagerUI.Instance.GetFirstOrderTemplate())
        {
            cookingTimeStartTime -= Time.deltaTime;
            timerImage.fillAmount = 1 - (cookingTimeStartTime / cookingTime);
            if (cookingTime < 0f) { 
               // OrderManagerUI.Instance.RemoveFirstOrderTemplate();
            }
        }
    }
}
