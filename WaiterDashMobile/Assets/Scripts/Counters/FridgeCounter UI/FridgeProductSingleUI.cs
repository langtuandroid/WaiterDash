using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Counters.FridgeCounter_UI
{
    public class FridgeProductSingleUI : MonoBehaviour
    {
        //[SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI productName;

        public void setProduct(RestaurantObjectSO itemData)
        {
            //itemIcon.sprite = itemData.sprite;
            productName.text = itemData.name;
            
            this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                Player player = Player.Instance;
                if (!player.HasRestaurantObject())
                {
                    RestaurantObject.SpawnRestaurantObject(itemData, player);
                    FridgeCounterUI.Instance.closeFridgeCounterUI();
                }
            });
        }
    }

}
