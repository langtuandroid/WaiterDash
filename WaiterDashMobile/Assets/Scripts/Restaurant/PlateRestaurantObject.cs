using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateRestaurantObject : RestaurantObject, IRestaurantObjectParent
{
    [SerializeField] private List<RestaurantObjectSO> validRestaurantObjectSO;
    [SerializeField] private Transform plateTopPosition;

    public List<RestaurantObjectSO> restaurantObjectSOList;
    private RestaurantObject restaurantObject;

    private void Awake()
    {
        restaurantObjectSOList = new List<RestaurantObjectSO>();
    }

    public bool TryAddIngredient(RestaurantObjectSO restaurantObjectSO)
    {
        if(restaurantObjectSOList.Contains(restaurantObjectSO))
        {
            return false;
        } else
        {
            restaurantObjectSOList.Add(restaurantObjectSO);
            return true;
        }
    }

    public Transform GetRestaurantObjectFollowTransform()
    {
        return plateTopPosition;
    }

    public void SetRestaurantObject(RestaurantObject restaurantObject)
    {
        this.restaurantObject = restaurantObject;
    }

    public RestaurantObject GetRestaurantObject()
    {
        return restaurantObject;
    }

    public void ClearRestaurantObject()
    {
        restaurantObject = null;
        restaurantObjectSOList.Clear();
    }

    public bool HasRestaurantObject()
    {
        return restaurantObject != null;
    }
}
