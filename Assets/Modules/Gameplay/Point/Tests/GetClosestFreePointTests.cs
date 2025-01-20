using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Gameplay
{
    [TestFixture]
    public sealed class GetClosestFreePointTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void Test(
            Point<string>[] slots,
            Vector3 center,
            bool exprectedSucess,
            string exprectedSlotName
        )
        {
            //Act:
            bool actualSuccess = slots.TryGetClosestFreePoint(center, out Point<string> actualSlot);

            //Assert:
            Assert.AreEqual(exprectedSucess, actualSuccess);
            Assert.AreEqual(exprectedSlotName, actualSlot?.GetName());
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new BasePoint<string>("A1", new Vector3(0, 0, 0), Quaternion.identity),
                    new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity),
                    new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity),
                    new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity)
                },
                new Vector3(0, 0, 0),
                true,
                "A1"
            ).SetName("All slots are free");

            yield return new TestCaseData(
                new[]
                {
                    new BasePoint<string>("A1", new Vector3(0, 0, 0), Quaternion.identity, "Vasya"),
                    new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity, "Petya"),
                    new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity, "Masha"),
                    new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity, "Misha")
                },
                new Vector3(0, 0, 0),
                false,
                null
            ).SetName("All slots are occuped");
            
            yield return new TestCaseData(
                new[]
                {
                    new BasePoint<string>("A1", new Vector3(0, 0, 0), Quaternion.identity, "Vasya"),
                    new BasePoint<string>("A3", new Vector3(2, 0, 2), Quaternion.identity),
                    new BasePoint<string>("A2", new Vector3(1, 0, 1), Quaternion.identity, "Masha"),
                    new BasePoint<string>("A4", new Vector3(3, 0, 3), Quaternion.identity)
                },
                new Vector3(0, 0, 0),
                true,
                "A3"
            ).SetName("Some slots are occuped");
        }
    }
}