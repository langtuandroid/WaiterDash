using Assets.Scripts.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Counters.CounterUI
{
    public class OrderNoteOverview : MonoBehaviour
    {
        [SerializeField] public Image OrderIconTemplate;
        [SerializeField] public Transform OrdersContent;
        [SerializeField] public TextMeshProUGUI DiningTableName;
        private List<Image> orderIconTemplates;
        public static OrderNoteOverview Instance { get; set; }
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            OrderIconTemplate.gameObject.SetActive(false);
            orderIconTemplates = new List<Image>();
            this.gameObject.SetActive(false);
            OrdersOverview.OnOrderConfirmed += OnOrderedConfirmed;
        }

        private void OnOrderedConfirmed(object sender, EventArgs e)
        {
            hideOrderNoteOverview();
        }

        public void AddOrderNote(CustomerOrder customerOrder, BaseDiningTable diningTable)
        {
            this.gameObject.SetActive(true);
            DiningTableName.text = diningTable.name;
            var orders = new List<RestaurantObject>();
            if (customerOrder.FoodOrder)
            {
                orders.Add(customerOrder.FoodOrder);
            }

            if (customerOrder.DrinkOrder)
            {
                orders.Add(customerOrder.DrinkOrder);
            }
            foreach (RestaurantObject order in orders)
            {
                Image selectedOrderIcon = Instantiate(OrderIconTemplate, OrdersContent);
                orderIconTemplates.Add(selectedOrderIcon);
                TextMeshProUGUI orderQuantity = selectedOrderIcon.GetComponentInChildren<TextMeshProUGUI>();
                selectedOrderIcon.gameObject.SetActive(true);
                selectedOrderIcon.sprite = order.GetRestaurantObjectSO().sprite;
                orderQuantity.text = order.GetRestaurantObjectSO().orderQuantity.ToString();
            }
        }

        public void clearOrderIconTemplates()
        {
            foreach (Image child in orderIconTemplates)
            {
                if (child == OrderIconTemplate) continue;
                Destroy(child.gameObject);
            }
            orderIconTemplates.Clear();
            DiningTableName.text = "";
        }

        public void hideOrderNoteOverview()
        {
            this.gameObject.SetActive(false);
        }


    }

}
