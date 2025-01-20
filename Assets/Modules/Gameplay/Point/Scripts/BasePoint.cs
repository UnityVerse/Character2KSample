using UnityEngine;

namespace Modules.Gameplay
{
    public class BasePoint<T> : Point<T> where T : class
    {
        private Vector3 position;
        private Quaternion rotation;

        public BasePoint(T value = default) : base(string.Empty, value)
        {
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
        }

        public BasePoint(string name, Vector3 position, T value = default) :
            base(name, value)
        {
            this.position = position;
            this.rotation = Quaternion.identity;
        }
        
        public BasePoint(string name, Vector3 position, Quaternion rotation, T value = default) :
            base(name, value)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public void SetRotation(Quaternion rotatiton)
        {
            this.rotation = rotatiton;
        }

        public override Vector3 GetPosition()
        {
            return this.position;
        }

        public override Quaternion GetRotation()
        {
            return this.rotation;
        }
    }
}