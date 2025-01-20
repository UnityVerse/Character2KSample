using System.Collections.Generic;
using NUnit.Framework;

namespace Modules.Gameplay
{
    [TestFixture]
    public sealed class StorageTests
    {
        private MutableStorage<string> storage;

        [SetUp]
        public void Setup()
        {
            storage = new MutableStorage<string>();
        }

        [Test]
        [TestCase("Boots", 2, ExpectedResult = 2)]
        [TestCase("Apple", 0, ExpectedResult = 0)]
        [TestCase("Sword", 1, ExpectedResult = 1)]
        public int AddItemsTest(string itemId, int amount)
        {
            //Act:
            storage.AddAmount(itemId, amount);

            //Assert:
            return storage.GetAmount(itemId);
        }

        [Test]
        [TestCase("Apple", 5, 3, ExpectedResult = 2)]
        [TestCase("Banana", 5, -1, ExpectedResult = 5)]
        [TestCase("Milk", 5, 6, ExpectedResult = 5)]
        [TestCase("Sword", 1, 1, ExpectedResult = 0)]
        public int RemoveItems(string itemId, int initialCount, int removeCount)
        {
            //Arrange:
            storage = new MutableStorage<string>(new Dictionary<string, int>
            {
                {itemId, initialCount}
            });

            //Act:
            storage.RemoveAmount(itemId, removeCount);
            return storage.GetAmount(itemId);
        }

        [Test]
        public void HasItems()
        {
            //Act:
            storage = new MutableStorage<string>(new Dictionary<string, int>
            {
                {"Apple", 3},
                {"Banana", 2},
                {"Meat", 5}
            });

            Assert.IsTrue(storage.ExistsAny("Apple"));
            Assert.IsTrue(storage.ExistsAmount("Banana", 2));
            Assert.IsTrue(storage.ExistsAmount("Meat", 1));
        }

        [Test]
        public void ClearItems()
        {
            //Arrange:
            storage = new MutableStorage<string>(new Dictionary<string, int>
            {
                {"Apple", 3},
                {"Banana", 2},
                {"Meat", 5}
            });

            //Act:
            storage.Clear();
            
            Assert.IsFalse(storage.ExistsAny("Apple"));
            Assert.IsFalse(storage.ExistsAny("Banana"));
            Assert.IsFalse(storage.ExistsAny("Meat"));
        }
    }
}