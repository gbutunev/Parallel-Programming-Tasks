using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace T3_PriceScraper
{
    public static class Scraper
    {
        public static void ScrapePrices(string[] urls)
        {
            List<Task<string>> taskList = new List<Task<string>>();
            foreach (var url in urls)
            {
                ScrapeTask st = new ScrapeTask(url);
                taskList.Add(Task.Run(() => st.Run()));
            }


            string output = string.Empty;
            Task.WhenAll(taskList)
                .ContinueWith(tasks => output = string.Join(Environment.NewLine, tasks.Result))
                .Wait();

            Console.WriteLine(output);
        }

    }

    public class ScrapeTask
    {
        public string Url { get; set; }
        public ScrapeTask(string url)
        {
            Url = url;
        }
        public string Run()
        {
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(Url);

                var code = doc.DocumentNode.SelectSingleNode("//div[@class='main-container-outer']/div[2]/div/section/div/div/div[2]/div/span").InnerText;
                var intPrice = doc.DocumentNode.SelectSingleNode("//p[@class='product-new-price has-deal']/text()").InnerText;
                var fractionPrice = doc.DocumentNode.SelectSingleNode("//p[@class='product-new-price has-deal']/sup").InnerText;
                var currency = doc.DocumentNode.SelectSingleNode("//p[@class='product-new-price has-deal']/span").InnerText;
                var name = doc.DocumentNode.SelectSingleNode("//div[@class='main-container-outer']/div[2]/div/section/div/div/h1").InnerText;

                return $"{code.Trim()} - {intPrice.Trim()}.{fractionPrice.Trim()}{currency} - {name}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
    }
}
