using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitContainerCounter : BaseCounter
{
    [SerializeField] private FruitObjectSO fruitObjectSO;
    public event EventHandler OnFruitContainerCounterInteract;
    public static FruitContainerCounter Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (!player.HasRestaurantObject())
        {
            OnFruitContainerCounterInteract?.Invoke(player, EventArgs.Empty);
        }
    }
}
