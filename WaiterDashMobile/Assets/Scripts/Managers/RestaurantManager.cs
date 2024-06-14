using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class RestaurantManager : MonoBehaviour
    {
        public static RestaurantManager Instance;
        public static EventHandler OnRestaurantClosed;
        public static EventHandler OnRestaurantOpened;
        private bool restaurantStatus = false;
        // TODO :
        // Open Restaurant Make Player Moveable and Customer AI
        // Close Restaurant Make User able to drag and move scene to select object and interact with it.
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            CloseRestaurant();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                restaurantStatus = true;
                OnRestaurantOpened?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetKeyUp(KeyCode.L))
            {
                restaurantStatus = false;
                OnRestaurantClosed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OpenRestaurant()
        {
            restaurantStatus = true;
            OnRestaurantOpened?.Invoke(this, EventArgs.Empty);
        }

        public void CloseRestaurant()
        {
            restaurantStatus = false;
            OnRestaurantClosed?.Invoke(this, EventArgs.Empty);
        }

        public bool IsRestaurantOpen()
        {
            return restaurantStatus;
        }
    }
}
