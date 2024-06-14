using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenSinkCounterTimer : MonoBehaviour
{

    [SerializeField] Slider dishWashingTimer;

    private void Start()
    {
        dishWashingTimer.gameObject.SetActive(false);
        KitchenSinkCounter.OnWashingDishes += KitchenSinkCounter_OnWashingDishes;
    }

    private void KitchenSinkCounter_OnWashingDishes(object sender, EventArgs e)
    {
        var dishWashingTimerNormalized = (float)sender + 0.01f;
        dishWashingTimer.value = dishWashingTimerNormalized;

        // Check if the blender timer is at its extremes
        if (dishWashingTimerNormalized <= 0f || dishWashingTimerNormalized >= 1f)
        {
            dishWashingTimer.gameObject.SetActive(false); // Hide the slider
        }
        else
        {
            dishWashingTimer.gameObject.SetActive(true); // Show the slider
        }
    }

    private void Update()
    {
        //// Update the slider value based on the normalized blender timer
        //dishWashingTimer.value = KitchenSinkCounter.Instance.GetWashingTimerNormalized();

        //// Check if the blender timer is at its extremes
        //if (KitchenSinkCounter.Instance.GetWashingTimerNormalized() <= 0f || KitchenSinkCounter.Instance.GetWashingTimerNormalized() >= 1f)
        //{
        //    dishWashingTimer.gameObject.SetActive(false); // Hide the slider
        //}
        //else
        //{
        //    dishWashingTimer.gameObject.SetActive(true); // Show the slider
        //}
    }
}
