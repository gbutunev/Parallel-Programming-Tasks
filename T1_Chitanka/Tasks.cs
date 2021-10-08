using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace T1_Chitanka
{
    public static class Tasks
    {
        private static Stopwatch stopwatch = new Stopwatch();

        public static long RunSynchronous(List<string> words)
        {
            stopwatch.Restart();

            PrintNumberOfWords(words);
            PrintShortestWord(words);
            PrintLongestWord(words);
            PrintAverageWordLength(words);
            PrintFiveMostCommonWords(words);
            PrintFiveLeastCommonWords(words);

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long RunParallel(List<string> words)
        {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(PrintNumberOfWords));
            threads.Add(new Thread(PrintShortestWord));
            threads.Add(new Thread(PrintLongestWord));
            threads.Add(new Thread(PrintAverageWordLength));
            threads.Add(new Thread(PrintFiveMostCommonWords));
            threads.Add(new Thread(PrintFiveLeastCommonWords));

            stopwatch.Restart();
            foreach (var thread in threads) thread.Start(words);

            foreach (var thread in threads) thread.Join();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private static void PrintShortestWord(object list)
        {
            List<string> words = (List<string>)list;

            int currLength = int.MaxValue;
            string currWord = string.Empty;

            foreach (string word in words)
            {
                if (word.Length < currLength)
                {
                    currLength = word.Length;
                    currWord = word;
                }
            }

            Console.WriteLine($"Shortest word is \"{currWord}\" with {currLength} characters");
        }

        private static void PrintFiveLeastCommonWords(object list)
        {
            List<string> words = (List<string>)list;

            string[] top5 = words.GroupBy(w => w)
                            .OrderBy(gr => gr.Count())
                            .Take(5)
                            .Select(gr => gr.Key)
                            .ToArray();

            string output = "Top 5 least common words:\n";
            for (int i = 0; i < 5; i++)
            {
                output += $"{i + 1}. {top5[i]}\n";
            }

            Console.WriteLine(output);
        }

        private static void PrintFiveMostCommonWords(object list)
        {
            List<string> words = (List<string>)list;

            string[] top5 = words.GroupBy(w => w)
                                        .OrderByDescending(gr => gr.Count())
                                        .Take(5)
                                        .Select(gr => gr.Key)
                                        .ToArray();

            string output = "Top 5 most common words:\n";
            for (int i = 0; i < 5; i++)
            {
                output += $"{i + 1}. {top5[i]}\n";
            }

            Console.WriteLine(output);
        }

        private static void PrintAverageWordLength(object list)
        {
            List<string> words = (List<string>)list;

            double length = words.Average(w => w.Length);
            Console.WriteLine($"Average word length is {length:F2}");
        }

        private static void PrintLongestWord(object list)
        {
            List<string> words = (List<string>)list;

            int currLength = 0;
            string currWord = string.Empty;

            foreach (string word in words)
            {
                if (word.Length > currLength)
                {
                    currLength = word.Length;
                    currWord = word;
                }
            }

            Console.WriteLine($"Longest word is \"{currWord}\" with {currLength} characters");
        }

        private static void PrintNumberOfWords(object list)
        {
            List<string> words = (List<string>)list;

            Console.WriteLine($"Total number of words: {words.Count}");
        }
    }
}
