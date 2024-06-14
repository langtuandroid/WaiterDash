using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace Assets.Scripts.Counters.CounterUI
{
    public class OrdersOverview : MonoBehaviour
    {
        [SerializeField] public Image OrderIconTemplate;
        [SerializeField] public TextMeshProUGUI totalPrice;

        private List<Image> ordersIcon = new List<Image>();
        private List<RestaurantObject> selectedOrders = new List<RestaurantObject>();

        public static event EventHandler OnOrderConfirmed;
        public static OrdersOverview Instance { get; set; }
        private int priceCount;

        private void Awake()
        {
            Instance = this;
            priceCount = 0;
        }
        private void Start()
        {
            OrderIconTemplate.gameObject.SetActive(false);
        }

        public void clearIcons()
        {
            selectedOrders = new List<RestaurantObject>();
            foreach (Image child in ordersIcon)
            {
                if (child == OrderIconTemplate) continue;
                Destroy(child.gameObject);
            }
            ordersIcon.Clear();
            priceCount = 0;
            totalPrice.text = priceCount.ToString();
        }

        public void AddSelectedItem(RestaurantObject restaurantObject)
        {
            RestaurantObject existingItem = selectedOrders.FirstOrDefault(item => item.Equals(restaurantObject));
            if (existingItem != null)
            {
                // If it exists, increment the order quantity
                int index = selectedOrders.IndexOf(existingItem);
                existingItem.GetRestaurantObjectSO().orderQuantity++;
                TextMeshProUGUI orderQuantity = ordersIcon[index].GetComponentInChildren<TextMeshProUGUI>();
                orderQuantity.text = existingItem.GetRestaurantObjectSO().orderQuantity.ToString();
            }
            else
            {
                // If it doesn't exist, add it to the list
                restaurantObject.GetRestaurantObjectSO().orderQuantity = 1; // Set initial order quantity
                selectedOrders.Add(restaurantObject);
                // Create a new order icon
                Image selectedOrderIcon = Instantiate(OrderIconTemplate, this.transform);
                TextMeshProUGUI orderQuantity = selectedOrderIcon.GetComponentInChildren<TextMeshProUGUI>();
                Button removeOrderButton = selectedOrderIcon.GetComponentInChildren<Button>();
                removeOrderButton.onClick.AddListener(() =>
                {
                    RemoveSelectedItem(restaurantObject);
                });
                orderQuantity.text = "1";
                selectedOrderIcon.gameObject.SetActive(true);
                selectedOrderIcon.sprite = restaurantObject.GetRestaurantObjectSO().sprite;
                ordersIcon.Add(selectedOrderIcon);
            }
            priceCount = priceCount + restaurantObject.GetRestaurantObjectSO().price;
            totalPrice.text = priceCount.ToString();
        }

        private void RemoveSelectedItem(RestaurantObject restaurantObject)
        {
            RestaurantObject existingItem = selectedOrders.FirstOrDefault(item => item.Equals(restaurantObject));
            if (existingItem != null)
            {
                int index = selectedOrders.IndexOf(existingItem);
                existingItem.GetRestaurantObjectSO().orderQuantity--;
                if (existingItem.GetRestaurantObjectSO().orderQuantity <= 0)
                {
                    selectedOrders.RemoveAt(index);
                    Destroy(ordersIcon[index].gameObject);
                    ordersIcon.RemoveAt(index);
                }
                else
                {
                    TextMeshProUGUI orderQuantity = ordersIcon[index].GetComponentInChildren<TextMeshProUGUI>();
                    orderQuantity.text = existingItem.GetRestaurantObjectSO().orderQuantity.ToString();
                }
            }
            priceCount = priceCount - restaurantObject.GetRestaurantObjectSO().price;
            totalPrice.text = priceCount.ToString();
        }

        public void ConfirmOrder()
        {
            if(selectedOrders.Count > 0)
            {
                CustomerOrdersUI.Instance.AddCustomerOrderUI(selectedOrders);
                OnOrderConfirmed?.Invoke(this, EventArgs.Empty);
                GoBack();
            }
        }
        public void GoBack()
        {
            CasaTableUI.Instance.loadTableInCasaCounter();
        }
    }
}
