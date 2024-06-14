using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitObject : RestaurantObject
{
    [SerializeField] private FruitObjectSO fruitObjectSO;


    public FruitObjectSO GetFruitObjectSO()
    {
        return fruitObjectSO;
    }
    public static RestaurantObject SpawnFruitObject(FruitObjectSO fruitObjectSO, IRestaurantObjectParent restaurantObjectParent)
    {
        Transform fruitObjectSOTransform = Instantiate(fruitObjectSO.prefab);
        FruitObject fruitObject = fruitObjectSOTransform.GetComponent<FruitObject>();
        fruitObject.SetRestaurantObjectParent(restaurantObjectParent, restaurantObjectParent.GetRestaurantObjectFollowTransform());
        return fruitObject;
    }
}
