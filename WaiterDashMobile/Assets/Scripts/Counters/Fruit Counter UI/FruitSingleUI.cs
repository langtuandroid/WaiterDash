using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitName;
    private FruitObject fruitRestaurantObject;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            Player player = Player.Instance;
            if (!player.HasRestaurantObject())
            {
                FruitObject.SpawnFruitObject(fruitRestaurantObject.GetFruitObjectSO(), player);
                FruitContainerCounterUI.Instance.closeFruitContainerCounterUI();
            }
        });
    }

    public void SetRestaurantObject(FruitObject fruitRestaurantObject)
    {
        this.fruitRestaurantObject = fruitRestaurantObject;
        fruitName.text = fruitRestaurantObject.GetFruitObjectSO().objectName;
    }
}
