using System;
using System.Collections.Generic;
using System.Text;

namespace T2_OnlineShop
{
    public class Store
    {
        private Dictionary<int, int> warehouse = new Dictionary<int, int>();
        private object locker = new object();

        public bool AddOrIncreaseQuantity(int product, int amount)
        {
            lock (locker)
            {
                if (warehouse.ContainsKey(product))
                    warehouse[product] += amount;
                else
                    warehouse[product] = amount;
                return true;
            }
        }

        public bool Buy(int product, int amount)
        {
            if (!warehouse.ContainsKey(product)) return false;
            if (warehouse[product] < amount) return false;

            lock (locker)
            {
                warehouse[product] -= amount;
            }
            return true;
        }

        public int GetQuantity(int product)
        {
            if (warehouse.ContainsKey(product))
            {
                return warehouse[product];
            }
            return -1;
        }
    }
}
