using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Customers
{
    public class CustomerOrder
    {
        public RestaurantObject FoodOrder { get; set; }
        public RestaurantObject DrinkOrder { get; set; }
    }
}
