using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ItemListManager : MonoBehaviour
    {
        [SerializeField] private List<RestaurantObjectSO> FridgeProducts;
        public static ItemListManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        public List<RestaurantObjectSO> GetFridgeProducts()
        {
            return FridgeProducts;
        }
    }

}
