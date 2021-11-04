using System;
using System.IO;

namespace T3_PriceScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string LIST_URL = "https://www.emag.bg/mobilni-telefoni/c?ref=hp_menu_quick-nav_1_1&type=category";
            const string SELECTOR = "//div[@class='card-item card-standard js-product-data']/div/div/div[3]/a";
            Utilities.GenerateCSV(LIST_URL, SELECTOR);

            Console.WriteLine("Loading CSV...");
            string[] urls = File.ReadAllLines("./links.csv");
            Scraper.ScrapePrices(urls);

            //Could not fix:
            //random new lines in output
            //some of the products had different html structure and returned null
        }
    }
}
