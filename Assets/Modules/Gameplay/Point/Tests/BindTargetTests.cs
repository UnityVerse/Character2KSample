using NUnit.Framework;
using UnityEngine;

namespace Modules.Gameplay
{
    [TestFixture]
    public sealed class BindValueTests
    {
        [Test]
        public void BindValue()
        {
            //Arrange:
            Point<string>[] slots =
            {
                new BasePoint<string>("A1", new Vector3(0.25f, 0, 0.25f), Quaternion.identity),
                new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity),
                new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity),
                new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity)
            };

            //Act:
            bool success = slots.BindValue("Vasya", Vector3.zero, out Point<string> result);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual("A1", result?.GetName());
        }

        [Test]
        public void WhenBindTargetWithoutFreeSlotsThenReturnFalse()
        {
            //Arrange:
            const string target = "Vasya";

            Point<string>[] slots =
            {
                new BasePoint<string>("A1", new Vector3(0, 0, 0), Quaternion.identity, "1"),
                new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity, "2"),
                new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity, "3"),
                new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity, "4")
            };

            //Act:
            bool success = slots.BindValue(target, Vector3.zero, out Point<string> result);

            //Assert:
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [Test]
        public void WhenBindTargetThatAlreadyBoundThenReturnFalse()
        {
            //Arrange:
            const string target = "Vasya";

            Point<string>[] slots =
            {
                new BasePoint<string>("A1", new Vector3(0.25f, 0, 0.25f), Quaternion.identity),
                new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity),
                new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity, target),
                new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity)
            };

            //Act:
            bool success = slots.BindValue(target, Vector3.zero, out Point<string> result);

            //Assert:
            Assert.IsFalse(success);
            Assert.AreEqual("A2", result?.GetName());
        }

        [Test]
        public void UnbindTarget()
        {
            //Arrange:
            const string target = "Vasya";

            Point<string>[] slots =
            {
                new BasePoint<string>("A1", new Vector3(0.25f, 0, 0.25f), Quaternion.identity),
                new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity),
                new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity, target),
                new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity)
            };

            //Act:
            bool success = slots.UnbindValue(target, out Point<string> result);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual("A2", result?.GetName());
        }

        [Test]
        public void WhenUnbindAbsentTargetThenReturnFalse()
        {
            //Arrange:
            const string target = "Vasya";

            Point<string>[] slots =
            {
                new BasePoint<string>("A1", new Vector3(0.25f, 0, 0.25f), Quaternion.identity),
                new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity),
                new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity),
                new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity)
            };

            //Act:
            bool success = slots.UnbindValue(target, out Point<string> result);

            //Assert:
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }
}