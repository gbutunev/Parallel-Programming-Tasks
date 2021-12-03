using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace T5_Area51
{
    class Elevator
    {
        public Floor currentFloor { get; private set; } = Floor.g;
        public bool IsOccupied
        {
            get
            {
                return currentAgent != null;
            }
        }
        Semaphore semaphore = new Semaphore(1, 1);
        Agent currentAgent = null;

        public void CallAndEnter(Agent agent)
        {
            semaphore.WaitOne();
            currentAgent = agent;
            int time = Math.Abs(currentFloor - agent.CurrentFloor) * 1000;
            Thread.Sleep(time);
            currentFloor = agent.CurrentFloor;
        }

        public bool GoToFloor(Floor floor)
        {
            int time = Math.Abs(currentFloor - floor) * 1000;
            Thread.Sleep(time);
            currentFloor = floor;
            currentAgent.CurrentFloor = floor;

            if (currentAgent.SecurityLevel == SecurityLevel.Topsecret) return true;
            if ((int)currentAgent.SecurityLevel >= (int)currentFloor) return true;
            return false;
        }

        public void Leave()
        {
            currentAgent = null;
            semaphore.Release();
        }
    }
}
