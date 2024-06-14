using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlenderCounterTimer : MonoBehaviour
{

    [SerializeField] private Slider blenderTimerSlider;

    private void Start()
    {
        blenderTimerSlider.gameObject.SetActive(false);
    }
    private void Update()
    {
            // Update the slider value based on the normalized blender timer
            blenderTimerSlider.value = DrinkCounter.Instance.GetBlendTimerNormalized() + 0.1f;

            // Check if the blender timer is at its extremes
            if (DrinkCounter.Instance.GetBlendTimerNormalized() <= 0f || DrinkCounter.Instance.GetBlendTimerNormalized() >= 1f)
            {
                blenderTimerSlider.gameObject.SetActive(false); // Hide the slider
            }
            else
            {
                blenderTimerSlider.gameObject.SetActive(true); // Show the slider
            }
    }
}
