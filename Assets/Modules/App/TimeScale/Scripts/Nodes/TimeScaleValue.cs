using System;

namespace Modules.App
{
    [Serializable]
    public sealed class TimeScaleValue : ITimeScaleNode
    {
        public string Name => this.name;

        public string name;
        public float scale;

        public float EvaluateScale()
        {
            return this.scale;
        }

        public TimeScaleValue()
        {
        }

        public TimeScaleValue(string name, float scale)
        {
            this.name = name;
            this.scale = scale;
        }
        
        public T FindChild<T>(string name) where T : class, ITimeScaleNode
        {
            return null;
        }

        public ITimeScaleNode FindChild(string name)
        {
            return null;
        }
    }
}