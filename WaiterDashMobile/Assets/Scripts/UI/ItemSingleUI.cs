using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ItemSingleUI : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image timerImage;
        [SerializeField] private TextMeshProUGUI quantity;
        private RestaurantObject restaurantObject;
        private float cookingTimeStartTime;
        private float cookingTime;
        private int itemQuantity;
        private int complementedQuantity;
        private Color cookingColor = Color.red;
        private Color cookingCompleteColor = Color.green;
        public void setItem(RestaurantObject restaurantObject)
        {
            this.restaurantObject = restaurantObject;
            var restaurantObjectSO = restaurantObject.GetRestaurantObjectSO();
            this.cookingTimeStartTime = restaurantObjectSO.cookingTime;
            this.cookingTime = restaurantObjectSO.cookingTime;
            itemIcon.sprite = restaurantObjectSO.sprite;
            itemQuantity = restaurantObjectSO.orderQuantity;
            complementedQuantity = 0;
            if (restaurantObjectSO.objectType == ObjectType.Food)
            {
                quantity.text = complementedQuantity.ToString() + "/" + itemQuantity.ToString();
            } else
            {
                quantity.text = itemQuantity.ToString();
            }
            timerImage.color = cookingColor;
        }


        private void Update()
        {
            if (!KitchenFoodCounter.Instance.HasRestaurantObject() && restaurantObject != null && KitchenManager.Instance.HasItemSingleUI())
            {
                if (this as ItemSingleUI == KitchenManager.Instance.GetFirstInQueueItemSingleUI())
                {
                    cookingTimeStartTime -= Time.deltaTime;
                    timerImage.fillAmount = 1 - (cookingTimeStartTime / cookingTime);
                    if (cookingTimeStartTime < 0f && itemQuantity != complementedQuantity)
                    {
                        complementedQuantity++;
                        quantity.text = complementedQuantity.ToString() + "/" + itemQuantity.ToString();
                        IsOrderItemCompleted();
                        if (IsOrderItemCompleted())
                        {
                            timerImage.color = cookingCompleteColor;
                            timerImage.fillAmount = 1;
                        } else
                        {
                            cookingTimeStartTime = cookingTime;
                        }
                    }
                }
            }
        }

        public bool IsOrderItemCompleted()
        {
            if (itemQuantity == complementedQuantity)
            {

                return true;
            } else
            {
                return false;
            }
        }
    }
}
