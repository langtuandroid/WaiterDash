using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRestaurantObjectParent
{
    public Transform GetRestaurantObjectFollowTransform();

    public void SetRestaurantObject(RestaurantObject restaurantObject);

    public RestaurantObject GetRestaurantObject();

    public void ClearRestaurantObject();

    public bool HasRestaurantObject();
}
