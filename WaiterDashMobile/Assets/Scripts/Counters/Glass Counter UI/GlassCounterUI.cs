using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCounterUI : MonoBehaviour
{
    [SerializeField] public List<RestaurantObject> Glasses;
    [SerializeField] public Transform GlassesContainer;
    [SerializeField] public Transform GlassTemplate;
    public static GlassCounterUI Instance;
    private void Awake()
    {
        Instance = this;
        GlassTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        GlassCounter.Instance.OnGlassCounterInteract += OnGlassCounterInteract;
        loadAvailableCrockeries();
    }

    private void OnGlassCounterInteract(object sender, System.EventArgs e)
    {
        
    }

    public void loadAvailableCrockeries()
    {
        foreach (Transform child in GlassesContainer)
        {
            if (child == GlassTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RestaurantObject restaurantObject in Glasses)
        {
            Transform restaurantObjectTransform = Instantiate(GlassTemplate, GlassesContainer);
            restaurantObjectTransform.gameObject.SetActive(true);
            restaurantObjectTransform.GetComponent<GlassSingleUI>().SetRestaurantObject(restaurantObject);
        }
    }

    public void closeGlassCounterUI()
    {
        this.gameObject.SetActive(false);
    }
}
