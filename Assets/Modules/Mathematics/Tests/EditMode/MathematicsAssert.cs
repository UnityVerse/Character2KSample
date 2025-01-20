using NUnit.Framework;
using UnityEngine;

public static class MathematicsAssert
{
    public static void AreEqual(float expected, float actual)
    {
        Assert.That(Mathf.Abs(expected - actual) < 0.001f);
    }
    
    public static void AreEqual(Vector3 expected, Vector3 actual)
    {
        Assert.That(Vector3.Distance(expected, actual) < 0.001f);
    }
}