using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBarSimulator
{
    class Student
    {
        enum NightlifeActivities { Walk, VisitBar, GoHome };
        enum BarActivities { Drink, Dance, Leave };

        Random random = new Random();

        public string Name { get; set; }
        public Bar Bar { get; set; }
        public double Money { get; set; }
        public int Age { get; set; }
        private NightlifeActivities GetRandomNightlifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightlifeActivities.Walk;
            if (n < 8) return NightlifeActivities.VisitBar;
            return NightlifeActivities.GoHome;
        }

        private BarActivities GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivities.Dance;
            if (n < 9) return BarActivities.Drink;
            return BarActivities.Leave;
        }

        private int GetRandomDrinkIndex()
        {
            return random.Next(Bar.DrinkMenuSize);
        }

        private void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
            Thread.Sleep(100);
        }

        private void VisitBar()
        {
            if (Bar.IsClosed)
            {
                Console.WriteLine($"{Name} goes to the bar but it is closed!");
                return;
            }

            Console.WriteLine($"{Name} is getting in the line to enter the bar.");
            Bar.EnterBarResult canEnter = Bar.Enter(this);

            switch (canEnter)
            {
                case Bar.EnterBarResult.AgeRestricted:
                    Console.WriteLine($"{Name} is not of legal age to enter the bar.");
                    return;
                case Bar.EnterBarResult.Closed:
                    Console.WriteLine($"{Name} saw that the bar was closing.");
                    return;
                case Bar.EnterBarResult.ChangePlans:
                    Console.WriteLine($"{Name} left the queue.");
                    return;
            }

            Console.WriteLine($"{Name} entered the bar!");
            bool staysAtBar = true;
            while (staysAtBar && !Bar.IsClosed)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivities.Dance:
                        Console.WriteLine($"{Name} is dancing.");
                        Thread.Sleep(100);
                        break;
                    case BarActivities.Drink:
                        Console.WriteLine($"{Name} goes to order something.");
                        string result = Bar.BuyDrink(this, GetRandomDrinkIndex());
                        Console.WriteLine($"{Name} bought {result ?? "nothing"}.");
                        Thread.Sleep(100);
                        break;
                    case BarActivities.Leave:
                        Console.WriteLine($"{Name} is leaving the bar.");
                        Bar.Leave(this);
                        staysAtBar = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
        }

        public void PaintTheTownRed()
        {
            WalkOut();
            bool staysOut = true;
            while (staysOut)
            {
                var nextActivity = GetRandomNightlifeActivity();
                switch (nextActivity)
                {
                    case NightlifeActivities.Walk:
                        WalkOut();
                        break;
                    case NightlifeActivities.VisitBar:
                        VisitBar();
                        staysOut = false;
                        break;
                    case NightlifeActivities.GoHome:
                        staysOut = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }

        public Student(string name, double money, int age, Bar bar)
        {
            Name = name;
            Money = money;
            Age = age;
            Bar = bar;
        }
    }
}
