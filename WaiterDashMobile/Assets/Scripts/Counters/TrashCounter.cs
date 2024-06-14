using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static EventHandler OnTrashThrown;
    public override void Interact(Player player)
    {
        if (player.HasRestaurantObject())
        {
            if (player.GetRestaurantObject() is PlateRestaurantObject)
            {
                PlateRestaurantObject plateRestaurantObject = player.GetRestaurantObject() as PlateRestaurantObject;
                if (plateRestaurantObject.HasRestaurantObject())
                {
                    plateRestaurantObject.GetRestaurantObject().DestorySelf();
                    plateRestaurantObject.ClearRestaurantObject();
                } else
                {
                    SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
                }
            } else
            {
                var restaurantObjectSO = player.GetRestaurantObject().GetRestaurantObjectSO();

                if (restaurantObjectSO.disposable == true)
                {
                    player.GetRestaurantObject().DestorySelf();
                    OnTrashThrown?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
                }
            }

        } else
        {
            SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
        }

    }

}
