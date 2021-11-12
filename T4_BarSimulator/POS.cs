using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4_BarSimulator
{
    public class POS
    {
        private StringBuilder log = new StringBuilder();
        private int purchaseCount = 0;
        private double turnover = 0;

        public void Log(Drink drink)
        {
            lock (log)
            {
                purchaseCount++;
                turnover += drink.Price;
                log.AppendLine($"Purchase {purchaseCount}: {drink.Name} - {drink.Price:F2}");
            }
        }

        public string GetLog()
        {
            return "\n" + log.ToString() + $"\nTotal income: {turnover:F2}";
        }
    }
}
