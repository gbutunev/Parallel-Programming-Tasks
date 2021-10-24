using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using T2_OnlineShop;

namespace T2_OnlineShopTests
{
    public class Tests
    {
        readonly Store store = new Store();

        [SetUp]
        public void Setup()
        {
            //load with initial quantity
            //for (int i = 0; i < 10; i++) store.AddOrIncreaseQuantity(i, 100);
        }

        [Test]
        public void Test1()
        {
            List<Thread> suppliers = new List<Thread>();
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(SupplyingWorker);
                t.Start();
                suppliers.Add(t);
            }
            List<Thread> buyers = new List<Thread>();
            for (int i = 0; i < 50; i++)
            {
                Thread t = new Thread(BuyingWorker);
                t.Start();
                buyers.Add(t);
            }

            foreach (var t in suppliers) t.Join();
            foreach (var t in buyers) t.Join();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(store.GetQuantity(i), 750);
            }
        }

        public void SupplyingWorker()
        {
            for (int i = 0; i < 10; i++)
                store.AddOrIncreaseQuantity(i, 100);

        }

        public void BuyingWorker()
        {
            for (int i = 0; i < 10; i++)
                store.Buy(i, 5);
        }
    }
}