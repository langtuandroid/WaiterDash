using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlassSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI glassName;
    private RestaurantObject glassRestaurantObject;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            Player player = Player.Instance;
            if (!player.HasRestaurantObject())
            {
                RestaurantObject.SpawnRestaurantObject(glassRestaurantObject.GetRestaurantObjectSO(), player);
                GlassCounterUI.Instance.closeGlassCounterUI();
            }
        });
    }


    public void SetRestaurantObject(RestaurantObject glassRestaurantObject)
    {
        this.glassRestaurantObject = glassRestaurantObject;
        glassName.text = glassRestaurantObject.GetRestaurantObjectSO().objectName;
    }
}
