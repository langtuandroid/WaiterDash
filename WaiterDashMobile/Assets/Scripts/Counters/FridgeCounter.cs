using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeCounter : BaseCounter
{
    public event EventHandler OnFridgeCounterInteract;
    public static FridgeCounter Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {

    }

    public override void Interact(Player player)
    {
        if(!player.HasRestaurantObject())
        {
            OnFridgeCounterInteract?.Invoke(player, EventArgs.Empty);
        }
    }
}
