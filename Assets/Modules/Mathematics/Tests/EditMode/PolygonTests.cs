using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;

namespace Modules.Mathematics
{
    [TestFixture]
    public sealed class PolygonTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void InPolygon(float3[] polygon, float3 point, bool result)
        {
            //Act:
            bool success = PolygonFunctions.InPolygonXZ(polygon, point);

            //Assert:
            Assert.AreEqual(result, success);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                new float3[]
                {
                    new(0, 5, -2),
                    new(3, 10, 0),
                    new(-2, 2, 1)
                },
                new float3(),
                true
            ).SetName("In Triangle (True)");

            yield return new TestCaseData(
                new float3[]
                {
                    new(0, 5, -2),
                    new(3, 10, 0),
                    new(-2, 2, 1)
                },
                new float3(20, 0, 1),
                false
            ).SetName("In Triangle (False)");
            
            yield return new TestCaseData(
                new float3[]
                {
                    new(0, 0, -2),
                    new(3, 10, 0),
                    new(-2, 2, 1)
                },
                new float3(0, 0, -2),
                false
            ).SetName("On Triangle (False)");
            
            yield return new TestCaseData(
                new[]
                {
                    new float3(-1, 0, 0),
                    new float3(-1, 0, 1),
                    new float3(1, 0, 2),
                    new float3(1, 0, 1)
                }, 
                new float3(0, 1, 1),
                true
            ).SetName("In Tetragon (True)");
            
            yield return new TestCaseData(
                new[]
                {
                    new float3(-1, 0, 0),
                    new float3(-1, 0, 1),
                    new float3(1, 0, 2),
                    new float3(1, 0, 1)
                }, 
                new float3(-1, 0, 0),
                false
            ).SetName("On Tetragon (False)");

            yield return new TestCaseData(
                new[]
                {
                    new float3(0, 0, -2),
                    new float3(-2, 0, 1),
                    new float3(-1, 0, 4),
                    new float3(3, 0, 3),
                    new float3(4, 0, 1),
                    new float3(3, 0, -2)
                }, 
                new float3(-1, 0, 0),
                true
            ).SetName("In Polygon (True)");
        }
    }
}