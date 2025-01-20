using System;

namespace Modules.Extensions
{
    public enum ComparisonMode
    {
        EQUALS = 0,
        LESS = 1,
        MORE = 2,
        LESS_OR_EQUALS = 3,
        MORE_OR_EQUALS = 4,
        NOT_EQUALS = 5
    }

    public static class ComparisonModeExtensions
    {
        public static bool Compare(this ComparisonMode mode, int first, int second)
        {
            return mode switch
            {
                ComparisonMode.EQUALS => first == second,
                ComparisonMode.LESS => first < second,
                ComparisonMode.MORE => first > second,
                ComparisonMode.LESS_OR_EQUALS => first <= second,
                ComparisonMode.MORE_OR_EQUALS => first >= second,
                ComparisonMode.NOT_EQUALS => first != second,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
        
        
        public static bool Compare(this ComparisonMode mode, float first, float second)
        {
            return mode switch
            {
                ComparisonMode.EQUALS => Math.Abs(first - second) < float.Epsilon,
                ComparisonMode.LESS => first < second,
                ComparisonMode.MORE => first > second,
                ComparisonMode.LESS_OR_EQUALS => first <= second,
                ComparisonMode.MORE_OR_EQUALS => first >= second,
                ComparisonMode.NOT_EQUALS => Math.Abs(first - second) > float.Epsilon,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
    }
}