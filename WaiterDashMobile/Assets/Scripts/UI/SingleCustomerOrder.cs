using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SingleCustomerOrder :MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TableText;
        [SerializeField] private Transform ordersContainer;
        [SerializeField] private ItemSingleUI orderTemplate;
        private BaseDiningTable diningTable;
        private List<ItemSingleUI> itemSingleUIList = new List<ItemSingleUI>();


        private void Start()
        {
            orderTemplate.gameObject.SetActive(false);
        }
        public void setDiningTable(BaseDiningTable diningTable)
        {
            this.diningTable = diningTable;
            TableText.text = diningTable.name;
        }

        public void setCustomerOrders(List<RestaurantObject> orderedItems)
        {
            foreach (RestaurantObject item in orderedItems)
            {
                ItemSingleUI itemUI = Instantiate(orderTemplate, ordersContainer);
                itemUI.gameObject.SetActive(true);
                itemUI.setItem(item);
                if (item.GetRestaurantObjectSO().objectType != ObjectType.Drinks)
                {
                    itemSingleUIList.Add(itemUI);
                }
            }
            KitchenManager.Instance.CookOrderedFood(orderedItems, itemSingleUIList);
        }

        public BaseDiningTable getDiningTable()
        {
            return diningTable;
        }
        public ItemSingleUI GetFirstItemSingleUI()
        {
            if (itemSingleUIList.Count <= 0)
            {
                return null;
            }
            else
            {
                return itemSingleUIList[0];
            }
        }

        public void destroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
