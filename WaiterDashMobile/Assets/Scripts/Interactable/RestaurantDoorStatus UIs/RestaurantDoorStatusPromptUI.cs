using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantDoorStatusPromptUI : MonoBehaviour
{
    [SerializeField] private Button close_button;
    [SerializeField] private Button closeRestaurant_button;
    [SerializeField] private Button back_button;
    private void Start()
    {
        close_button.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
        closeRestaurant_button.onClick.AddListener(() => {
            RestaurantManager.Instance.CloseRestaurant();
            this.gameObject.SetActive(false);
        });
        back_button.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
    }
}
