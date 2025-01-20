using System;
using Unity.Mathematics;

namespace Modules.Mathematics
{
    [Serializable]
    public readonly struct Line
    {
        public readonly float3 start;
        public readonly float3 end;

        public Line(float3 start, float3 end)
        {
            this.start = start;
            this.end = end;
        }

        public float3 GetVector()
        {
            return this.end - start;
        }
    }
}