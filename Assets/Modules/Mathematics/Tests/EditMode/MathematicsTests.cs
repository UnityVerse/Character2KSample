using NUnit.Framework;
using UnityEngine;

public sealed class MathematicsTests
{
    [Test]
    public void RaySegmentIntersect()
    {
        //Test#1
        var ray1 = new Ray(new Vector3(1, 0, 0), Vector3.forward);
        var start1 = new Vector3(0, 0, 2);
        var end1 = new Vector3(2, 0, 2);
        var success1 = Mathematics.RaySegmentIntersect(ray1, start1, end1, out var intersect1, out var distance1);
        Assert.True(success1);
        MathematicsAssert.AreEqual(2, distance1);
        MathematicsAssert.AreEqual(new Vector3(1, 0, 2), intersect1);

        //Test#2
        var ray2 = new Ray(new Vector3(4, 0, 0), new Vector3(-1, 0, 1));
        var start2 = new Vector3(2, 0, 0);
        var end2 = new Vector3(2, 0, 2);
        var success2 = Mathematics.RaySegmentIntersect(ray2, start2, end2, out var intersect2, out var distance2);
        Assert.True(success2);
        MathematicsAssert.AreEqual(Mathf.Sqrt(8), distance2);
        MathematicsAssert.AreEqual(new Vector3(2, 0, 2), intersect2);
        
        //Test#3
        var ray3 = new Ray(new Vector3(1, 0, 1), Vector3.forward);
        var start3 = new Vector3(0.5f, 0, 1.5f);
        var end3 = new Vector3(1.5f, 0, 1.5f);
        var success3 = Mathematics.RaySegmentIntersect(ray3, start3, end3, out var intersect3, out var distance3);
        Assert.True(success3);
        MathematicsAssert.AreEqual(0.5f, distance3);
        MathematicsAssert.AreEqual(new Vector3(1, 0, 1.5f), intersect3);
    }

    [Test]
    public void RayIntoSquareIntersect()
    {
        const float squareSize = 1.0f;

        //Test#1
        var ray1 = new Ray(new Vector3(1, 0, 1), Vector3.forward);
        var center1 = new Vector3(1, 0, 1);
        var success1 = Mathematics.RayIntoSquareIntersect(ray1, center1, squareSize, out var intersect1, out var distance1);
        Assert.True(success1);
        MathematicsAssert.AreEqual(new Vector3(1, 0, 1.5f), intersect1);
        MathematicsAssert.AreEqual(0.5f, distance1);
        
        //Test#2
        var ray2 = new Ray(new Vector3(0.25f, 0, 0.25f), new Vector3(1, 0, 1));
        var center2 = new Vector3(0.5f, 0, 0.5f);
        var success2 = Mathematics.RayIntoSquareIntersect(ray2, center2, squareSize, out var intersect2, out var distance2);
        Assert.True(success2);
        MathematicsAssert.AreEqual(new Vector3(1, 0, 1), intersect2);
    }
}


// var halfSize = squareSize / 2;
// var topLeft = square1 + new Vector3(-halfSize, 0, halfSize);
// var topRight = square1 + new Vector3(halfSize, 0, halfSize);

// Debug.Log($"TL {topLeft} TR {topRight}");
        
// var success1 = Mathematics.RaySegmentIntersect(ray1, topLeft, topRight, out var intersect1, out var distance1);
// Debug.Log($"{success1} {intersect1} {distance1}");

// var success1 = Mathematics.RayIntoSquareIntersect(ray1, square1, 2, out var intersect1, out var distance1);
