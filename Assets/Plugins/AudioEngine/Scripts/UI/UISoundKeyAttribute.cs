using System;

namespace AudioEngine
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class UISoundKeyAttribute : Attribute
    {
    }
}