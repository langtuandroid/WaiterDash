using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCounter : BaseCounter
{
    public event EventHandler OnGlassCounterInteract;
    public static GlassCounter Instance;

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
            Debug.Log("Glass Interact");
            OnGlassCounterInteract?.Invoke(player, EventArgs.Empty);
        }
    }
}
