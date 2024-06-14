using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenUI : MonoBehaviour
{
    [SerializeField] GameObject storeUI;
    [SerializeField] Button button_openStore;
    [SerializeField] Button button_openRestaurant;
    public static OnScreenUI Instance { get; set; }
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        RestaurantManager.OnRestaurantClosed += OnRestaurantClosed;
        RestaurantManager.OnRestaurantOpened += OnRestaurantOpened;
    }

    private void OnDisable()
    {
        RestaurantManager.OnRestaurantClosed -= OnRestaurantClosed;
        RestaurantManager.OnRestaurantOpened -= OnRestaurantOpened;
    }

    private void Start()
    {

        button_openStore.onClick.AddListener(() =>
        {
            OnButtonOpenStoreClicked();
        });

        button_openRestaurant.onClick.AddListener(() =>
        {
            RestaurantManager.Instance.OpenRestaurant();
        });

    }

    private void OnRestaurantClosed(object sender, EventArgs e)
    {
        button_openStore.gameObject.SetActive(true);
        button_openRestaurant.gameObject.SetActive(true);
    }

    private void OnRestaurantOpened(object sender, EventArgs e)
    {
        button_openStore.gameObject.SetActive(false);
        button_openRestaurant.gameObject.SetActive(false);
    }

    public void OnButtonOpenStoreClicked()
    {
        storeUI.SetActive(!storeUI.activeSelf);
    }

    public void hideStoreUI()
    {
        storeUI.SetActive(false);
    }
}
