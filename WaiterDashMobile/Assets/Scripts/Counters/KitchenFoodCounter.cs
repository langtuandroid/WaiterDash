using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenFoodCounter : BaseCounter
{
    [SerializeField] private Image foodIcon;
    [SerializeField] private Image background;
    public static KitchenFoodCounter Instance { get; set; }
    public EventHandler OnFoodReady;
    private List<RestaurantObject> restaurantObjectsQueue;
    private void Start()
    {
        Instance = this;
        background.gameObject.SetActive(false);
        foodIcon.gameObject.SetActive(false);
    }
    public override void Interact(Player player)
    {
        Debug.Log("KitchenFoodCounter");

        if (player.HasRestaurantObject())
        {
            if(player.GetRestaurantObject() is PlateRestaurantObject)
            {
                PlateRestaurantObject plateRestaurantObject = player.GetRestaurantObject() as PlateRestaurantObject;
                if(plateRestaurantObject.TryAddIngredient(GetRestaurantObject().GetRestaurantObjectSO()))
                {
                    var kitchenTableFood = GetRestaurantObject();
                    kitchenTableFood.SetRestaurantObjectParent(plateRestaurantObject, plateRestaurantObject.GetRestaurantObjectFollowTransform());
                }
            }            
            else
            {
                if (GetRestaurantObject().GetRestaurantObjectSO().objectType == ObjectType.Drinks)
                {
                    GetRestaurantObject().SetRestaurantObjectParent(player, player.GetRestaurantObjectFollowTransform()) ;
                }
                SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
            }
        } else
        {
            SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
        }
    }
    private void Update()
    {
        if(HasRestaurantObject())
        {
            background.gameObject.SetActive(true);
            foodIcon.gameObject.SetActive(true);
        } else
        {
            background.gameObject.SetActive(false);
            foodIcon.gameObject.SetActive(false);
        }
    }
    public void SpawnOrderedFood(RestaurantObject restaurantObject)
    {
        if(!HasRestaurantObject())
        {
            var restaurantObjectSO = restaurantObject.GetRestaurantObjectSO();
            RestaurantObject.SpawnRestaurantObject(restaurantObjectSO, this);
            foodIcon.sprite = restaurantObjectSO.sprite;
            OnFoodReady?.Invoke(this, EventArgs.Empty);
        }
    }


}
