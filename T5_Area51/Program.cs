using System;
using System.Collections.Generic;
using System.Threading;

namespace T5_Area51
{
    class Program
    {
        //confidential = g
        //secret = g,s
        //topsecret = g,s,t1,t2
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                Random random = new Random();

                Elevator elevator = new Elevator();
                List<Thread> agentThreads = new List<Thread>();
                for (int i = 1; i <= 10; i++)
                {
                    SecurityLevel securityLevel = (SecurityLevel)random.Next(3);
                    Agent agent = new Agent(i.ToString(), elevator, securityLevel);
                    Thread thread = new Thread(agent.Work);
                    thread.Start();
                    agentThreads.Add(thread);
                }

                foreach (var t in agentThreads) t.Join();
                Console.WriteLine("Workday is over.");
            }).Start();
        }
    }
}
