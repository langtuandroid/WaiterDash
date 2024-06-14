using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FruitContainerCounterUI : MonoBehaviour
{
    [SerializeField] public List<FruitObject> fruits;
    [SerializeField] public Transform FruitsContainer;
    [SerializeField] public Transform FruitTemplate;
    public static FruitContainerCounterUI Instance;

    private void Awake()
    {
        Instance = this;
        FruitTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        FruitContainerCounter.Instance.OnFruitContainerCounterInteract += OnFruitContainerCounterInteract;
        loadAvailableFruits();
    }

    private void OnFruitContainerCounterInteract(object sender, EventArgs e)
    {

    }

    private void loadAvailableFruits()
    {
        foreach (Transform child in FruitsContainer)
        {
            if (child == FruitTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (FruitObject restaurantObject in fruits)
        {
            Transform restaurantObjectTransform = Instantiate(FruitTemplate, FruitsContainer);
            restaurantObjectTransform.gameObject.SetActive(true);
            restaurantObjectTransform.GetComponent<FruitSingleUI>().SetRestaurantObject(restaurantObject);
        }
    }
    public void closeFruitContainerCounterUI()
    {
        this.gameObject.SetActive(false);
    }
}
