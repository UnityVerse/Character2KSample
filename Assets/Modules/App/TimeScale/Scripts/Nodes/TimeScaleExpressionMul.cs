using System;
using System.Collections.Generic;

namespace Modules.App
{
    [Serializable]
    public sealed class TimeScaleExpressionMul : TimeScaleNodeComposite
    {
        public TimeScaleExpressionMul()
        {
        }

        public TimeScaleExpressionMul(string name) : base(name)
        {
        }

        protected override float EvaluateScale(IReadOnlyList<ITimeScaleNode> children)
        {
            float result = 1;
            if (children.Count == 0)
            {
                return result;
            }

            for (int i = 0, count = children.Count; i < count; i++)
            {
                ITimeScaleNode node = children[i];
                result *= node.EvaluateScale();
            }

            return result;
        }
    }
}