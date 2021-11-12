using System;
using System.Collections.Generic;
using System.Threading;

namespace DBarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            Bar bar = new Bar();
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 1; i <= 100; i++)
            {
                int age = random.Next(15, 25); // 30% chance <18?
                double money = (double)random.Next(0, 100);

                Student student = new Student(i.ToString(), money, age, bar);
                Thread thread = new Thread(student.PaintTheTownRed);
                thread.Start();
                studentThreads.Add(thread);
            }

            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            Console.ReadLine();
        }
    }
}
