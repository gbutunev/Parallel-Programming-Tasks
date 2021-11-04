using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace T3_PriceScraper
{
    public static class Utilities
    {
        public static void GenerateCSV(string mainUrl, string linkSelector)
        {
            Console.WriteLine("Fetching URL list and generating a CSV...");

            if (File.Exists("./links.csv"))
            {
                Console.WriteLine("CSV already created. Skipping.");
                return;
            }

            var web = new HtmlWeb();
            var doc = web.Load(mainUrl);

            var links = doc.DocumentNode.SelectNodes(linkSelector)
                .Select(node => node.Attributes["href"].Value);

            var output = string.Join(Environment.NewLine, links);

            File.WriteAllText("./links.csv", output);
            Console.WriteLine("CSV file created.");
        }
    }
}
