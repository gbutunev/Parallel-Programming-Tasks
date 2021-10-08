using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace T1_Chitanka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string runningDir = Environment.CurrentDirectory;
            string bookDir = Directory.GetParent(runningDir).Parent.Parent.ToString();
            string[] allLines = File.ReadAllLines(bookDir + "\\Orwell-1984.txt");
            
            List<string> allWords = new List<string>();
            //HashSet<string> distinctWords = new HashSet<string>();

            Regex regex = new Regex(@"\.|,|\?|!|„|“|:|;| — | - | |\t|\(|\)|\[|\]|\*|{|}"); //needs a lot more improvement
            foreach (var line in allLines)
            {
                string input = line.ToLower();

                string[] words = regex.Split(input);

                foreach (var word in words)
                {
                    if (word.Trim() != string.Empty)
                    {
                        allWords.Add(word);
                        //distinctWords.Add(word);
                    }
                }
            }


            long time1 = Tasks.RunSynchronous(allWords);
            Console.WriteLine($"Time elapsed for synchronous tasks: {time1}\n\n\n");

            long time2 = Tasks.RunParallel(allWords);
            Console.WriteLine($"Time elapsed for parallel tasks: {time2}");

        }
    }
}
