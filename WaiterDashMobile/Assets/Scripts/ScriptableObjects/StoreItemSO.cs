using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StoreItemSO : ScriptableObject
{
    public string ItemName;
    public ItemType type;
    public Sprite icon;
    public Transform prefab;
    public int price;
    public int level = 1;
}

public enum ItemType
{
    Furniture,
    Machine,
    Decoration,
    Counters,
    KitchenAppliance,
    RestaurantAppliance
}
