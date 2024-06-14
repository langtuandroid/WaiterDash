using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RestaurantStatusOpened;
    [SerializeField] private TextMeshProUGUI RestaurantStatusClosed;

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
    }

    private void OnRestaurantOpened(object sender, EventArgs e)
    {
        RestaurantStatusOpened.gameObject.SetActive(true);
        RestaurantStatusClosed.gameObject.SetActive(false);
    }

    private void OnRestaurantClosed(object sender, EventArgs e)
    {
        if (RestaurantStatusOpened != null && RestaurantStatusClosed != null)
        {
            RestaurantStatusOpened.gameObject.SetActive(false);
            RestaurantStatusClosed.gameObject.SetActive(true);
        }
    }
}
