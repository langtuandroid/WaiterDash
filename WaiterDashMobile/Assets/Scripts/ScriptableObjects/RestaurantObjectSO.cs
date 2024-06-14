using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu()]
public class RestaurantObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
    public ObjectType objectType;
    public float cookingTime;
    public int price;
    public int orderQuantity;
    public bool disposable;
}

public enum ObjectType
{
    MenuCard,
    OrderNote,
    Food,
    Drinks,
    PaymentMachine,
    Crockery
}
