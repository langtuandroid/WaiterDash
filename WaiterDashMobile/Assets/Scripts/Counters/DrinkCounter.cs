using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkCounter : BaseCounter
{
    [SerializeField] private Transform ClearPrompt;
    [SerializeField] private Transform FruitSlot;
    [SerializeField] private Transform GlassSlot;
    [SerializeField] private Image FruitIcon;
    [SerializeField] private Image GlassIcon;
    [SerializeField] private Transform FruitJuiceIconContainer;
    [SerializeField] private Image FruitJuiceIcon;
    [SerializeField] private RestaurantObject juiceGlass;

    private bool fruitOnBlender;
    private bool glassOnCounter;
    private bool fruitJuiceBlended;
    public bool isBlending;
    private float currentBlendingTime = 0f;
    private RestaurantObjectSO fruitJuiceSO;

    public static DrinkCounter Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FruitSlot.gameObject.SetActive(false);
        GlassSlot.gameObject.SetActive(false);
        ClearPrompt.gameObject.SetActive(false);
        FruitJuiceIconContainer.gameObject.SetActive(false);
    }
    private void Update()
    {

    }
    public override void Interact(Player player)
    {
        ClearPrompt.gameObject.SetActive(false);

        if (isBlenderReady() && fruitJuiceBlended)
        {
            RestaurantObject.SpawnRestaurantObject(fruitJuiceSO, player);
            fruitOnBlender = false;
            glassOnCounter = false;
            fruitJuiceBlended = false;
            FruitJuiceIconContainer.gameObject.SetActive(false);
        }
        else
        {
            if (player.HasRestaurantObject())
            {

                if (player.GetRestaurantObject() is FruitObject)
                {
                    var fruit = player.GetRestaurantObject() as FruitObject;
                    if (fruitOnBlender != true)
                    {
                        FruitSlot.gameObject.SetActive(true);
                        FruitIcon.sprite = fruit.GetFruitObjectSO().sprite;
                        fruitJuiceSO = fruit.GetRestaurantObjectSO();
                        fruitOnBlender = true;
                        fruit.DestorySelf();
                        player.ClearRestaurantObject();
                        if (isBlenderReady())
                        {
                            blendFruitJuice(player);
                        }
                    }
                    //var drink = fruit.GetRestaurantObjectSO();
                    //RestaurantObject.SpawnRestaurantObject(drink, player);
                }
                else if (player.GetRestaurantObject() is RestaurantObject)
                {
                    var glass = player.GetRestaurantObject();
                    if (glass.GetRestaurantObjectSO() == juiceGlass.GetRestaurantObjectSO())
                    {
                        if (glassOnCounter != true)
                        {
                            GlassSlot.gameObject.SetActive(true);
                            GlassIcon.sprite = glass.GetRestaurantObjectSO().sprite;
                            glassOnCounter = true;
                            glass.DestorySelf();
                            player.ClearRestaurantObject();
                            if (isBlenderReady())
                            {
                                blendFruitJuice(player);
                            }
                        }
                    }
                }
            }
            else
            {
                if (!isBlending)
                {
                    if (glassOnCounter || fruitOnBlender)
                    {
                        ClearPrompt.gameObject.SetActive(true);
                    }
                }
            }
        }

    }

    public void blendFruitJuice(Player player)
    {
        currentBlendingTime = fruitJuiceSO.cookingTime;
        isBlending = true;
        FunctionUpdater.Create(() =>
        {
            currentBlendingTime -= Time.deltaTime;
            if (currentBlendingTime <= 0f)
            {
                isBlending = false;
                fruitJuiceBlended = true;
                FruitJuiceIconContainer.gameObject.SetActive(true);
                FruitJuiceIcon.sprite = fruitJuiceSO.sprite;
                clearBlender();
                fruitOnBlender = true;
                glassOnCounter = true;
            }
            return fruitJuiceBlended;
        });
    }

    public float GetBlendTimerNormalized()
    {
        if (fruitJuiceSO)
        {
            return 1 - (currentBlendingTime / fruitJuiceSO.cookingTime);

        } else
        {
            return 0;
        }
    }

    public void clearBlender()
    {
        FruitSlot.gameObject.SetActive(false);
        GlassSlot.gameObject.SetActive(false);
        ClearPrompt.gameObject.SetActive(false);
        fruitOnBlender = false;
        glassOnCounter = false;
    }
    public bool isBlenderReady()
    {
        return fruitOnBlender && glassOnCounter;
    }
}
