using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class RestaurantDoorStatus : BaseInteractablePrefab
{

    public override void Interact(Player player)
    {
        Debug.Log("Interacted Door");
    }
}
