using System;
using UnityEngine;

namespace Atomic.AI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class BlackboardKeyAttribute : PropertyAttribute
    {
    }
}