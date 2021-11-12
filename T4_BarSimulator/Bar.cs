using System;
using System.Collections.Generic;
using System.Threading;
using T4_BarSimulator;

namespace DBarSimulator
{
    class Bar
    {
        //didn't manage to fix this:
        //sometimes the last purchase line (x bought something) appears after the log (even though it is present in the log itself)
        public enum EnterBarResult { AgeRestricted, Closed, ChangePlans, Success }

        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);

        POS POS = new POS();
        Random random = new Random();

        List<Drink> DrinkStorage = new List<Drink>
        {
            new Drink("Beer", 2.4, 50),
            new Drink("Whisky", 5, 50),
            new Drink("Vodka", 4.4, 50),
            new Drink("10 Shots", 12.6, 50),
        };

        public bool IsClosed = false;
        //private object closeLock = new object(); //prevent people from entering after bar starts closing?
        public int DrinkMenuSize => DrinkStorage.Count;

        public EnterBarResult Enter(Student student)
        {
            //lock (closeLock)
            //{
                semaphore.WaitOne();

                //randomly leave the queue
                //although if it reached this point it means that the student is first in line?
                if (random.Next(10) == 0)
                {
                    semaphore.Release();
                    return EnterBarResult.ChangePlans;
                }

                if (IsClosed)
                {
                    semaphore.Release();
                    return EnterBarResult.Closed;
                }

                if (student.Age < 18)
                {
                    semaphore.Release();
                    return EnterBarResult.AgeRestricted;
                }

                lock (students)
                {
                    students.Add(student);
                    return EnterBarResult.Success;
                }
            //}
        }

        public void Leave(Student student)
        {
            if (IsClosed) return; //wait for the thread to be kicked out by Close();

            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }

        public string BuyDrink(Student student, int drinkNumber)
        {
            if (IsClosed) return null;

            if (student.Money < DrinkStorage[drinkNumber].Price)
            {
                if (!IsClosed) RandomlyClose();
                return null;
            }

            lock (DrinkStorage)
            {
                if (DrinkStorage[drinkNumber].Quantity == 0)
                {
                    if (!IsClosed) RandomlyClose();
                    return null;
                }

                student.Money -= DrinkStorage[drinkNumber].Price;
                DrinkStorage[drinkNumber].Quantity--;

                POS.Log(DrinkStorage[drinkNumber]);
            }

            if (!IsClosed) RandomlyClose();
            return DrinkStorage[drinkNumber].Name;
        }

        private void Close()
        {
            //lock (closeLock)
            //{
                Console.WriteLine("Bar is now closing.");

                lock (students)
                {
                    IsClosed = true;
                    while (students.Count > 0)
                    {
                        var currentStudent = students[0];
                        students.Remove(currentStudent);
                        semaphore.Release();
                        Console.WriteLine($"Security kicked out {currentStudent.Name}.");
                    }
                }

                Console.WriteLine("Bar is now closed. Here is the history of purchases:\n" + POS.GetLog());
            //}
        }
        public void RandomlyClose()
        {
            if (random.Next(20) == 0)
            {
                Close();
            }
        }
    }
}
