using System;
using System.Collections.Generic;
using System.Text;

namespace T5_Area51
{
    class Agent
    {
        public string Name { get; set; }
        public SecurityLevel SecurityLevel { get; }
        public Floor CurrentFloor { get; set; } = Floor.g; //always start at ground floor
        public Elevator Elevator { get; }

        enum Activity { DoWork, ChangeFloor, Leave };
        Random random = new Random();

        public Agent(string name, Elevator elevator, SecurityLevel securityLevel)
        {
            Name = name;
            Elevator = elevator;
            SecurityLevel = securityLevel;
        }

        public void Work()
        {
            Console.WriteLine($"{Name} arrived at work.");

            bool isWorking = true;
            while (isWorking)
            {
                Activity activity = GetRandomActivity();
                switch (activity)
                {
                    case Activity.DoWork:
                        Console.WriteLine($"{Name} does something on floor {CurrentFloor}.");
                        break;

                    case Activity.ChangeFloor:
                        lock (Elevator)
                        {
                            if (!Elevator.IsOccupied) UseElevator();
                        }
                        break;

                    case Activity.Leave:
                        if (CurrentFloor != Floor.g) UseElevator(true);
                        isWorking = false;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            Console.WriteLine($"{Name} left Area 51.");
        }

        private void UseElevator(bool leaveWork = false)
        {
            lock (Elevator)
            {
                Console.WriteLine($"{Name} calls the elevator.");
                Elevator.CallAndEnter(this);
                Console.WriteLine($"{Name} entered the elevator.");

                if (leaveWork)
                {
                    Console.WriteLine($"{Name} selects the ground floor.");
                    Elevator.GoToFloor(Floor.g);
                }
                else
                {
                    bool isAllowed = false;
                    do
                    {
                        Floor floor = GetRandomFloor();
                        Console.WriteLine($"{Name} selects floor {floor}.");
                        isAllowed = Elevator.GoToFloor(floor);
                        if (!isAllowed) Console.WriteLine($"{Name} can't access floor {floor}.");
                    }
                    while (!isAllowed);
                }

                Elevator.Leave();
                Console.WriteLine($"{Name} is now on floor {CurrentFloor}.");
            }
        }

        private Floor GetRandomFloor()
        {
            int n = random.Next(20);
            if (n < 6) return Floor.g;
            if (n < 11) return Floor.s;
            if (n < 16) return Floor.t1;
            return Floor.t2;
        }

        private Activity GetRandomActivity()
        {
            int n = random.Next(10);
            if (n < 4) return Activity.DoWork;
            if (n < 9) return Activity.ChangeFloor;
            return Activity.Leave;
        }
    }
}
