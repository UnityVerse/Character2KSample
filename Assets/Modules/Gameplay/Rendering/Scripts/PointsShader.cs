using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Gameplay
{
    public sealed class PointsShader
    {
        private static readonly int Centers = Shader.PropertyToID("_Centers");
        private static readonly int Number = Shader.PropertyToID("_Number");
        private static readonly int Radiuses = Shader.PropertyToID("_Radiuses");
        private static readonly int Color = Shader.PropertyToID("_Color");

        private readonly Material selectionMaterial;

        [ShowInInspector, ReadOnly]
        private readonly Vector4[] positions;

        [ShowInInspector, ReadOnly]
        private readonly float[] radiuses;

        [ShowInInspector, ReadOnly]
        private Color color;

        [ShowInInspector, ReadOnly]
        private int count;

        public PointsShader(Material material, int capacity)
        {
            this.selectionMaterial = material;
            this.positions = new Vector4[capacity];
            this.radiuses = new float[capacity];
        }
        
        public void Update()
        {
            this.selectionMaterial.SetColor(Color, this.color);
            this.selectionMaterial.SetFloatArray(Radiuses, this.radiuses);
            this.selectionMaterial.SetVectorArray(Centers, this.positions);
            this.selectionMaterial.SetInt(Number, this.count);
        }

        public void ChangePoint(int index, Vector3 position)
        {
            this.positions[index] = position;
        }

        public void AddPoint(Vector3 position, float radius, out int index)
        {
            index = this.count++;
            this.positions[index] = position;
            this.radiuses[index] = radius;
        }

        public void RemovePoint(int index)
        {
            this.count--;

            for (int i = index; i < this.count; i++)
            {
                int next = i + 1;
                this.positions[i] = this.positions[next];
                this.radiuses[i] = this.radiuses[next];
            }
        }
        
        public void ClearPoints()
        {
            this.count = 0;
        }

        public void ChangeColor(Color color)
        {
            this.color = color;
        }
    }
}