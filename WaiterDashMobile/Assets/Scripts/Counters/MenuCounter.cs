using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private RestaurantObjectSO restaurantObjectSO;

    public override void Interact(Player player)
    {
        if(!player.HasRestaurantObject())
        {
            RestaurantObject.SpawnRestaurantObject(restaurantObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            player.GetRestaurantObject().DestorySelf();
        }
    }
}
