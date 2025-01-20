using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.App
{
    [Serializable]
    public abstract class TimeScaleNodeComposite : ITimeScaleNode
    {
        public string Name => this.name;

        [SerializeField]
        private string name;

        private readonly List<ITimeScaleNode> nodes = new();

        protected abstract float EvaluateScale(IReadOnlyList<ITimeScaleNode> children);

        protected TimeScaleNodeComposite()
        {
        }

        protected TimeScaleNodeComposite(string name)
        {
            this.name = name;
        }

        public bool AddChild(ITimeScaleNode node)
        {
            if (!this.nodes.Contains(node))
            {
                this.nodes.Add(node);
                return true;
            }

            return false;
        }

        public bool DelChild(ITimeScaleNode node)
        {
            return this.nodes.Remove(node);
        }

        public float EvaluateScale()
        {
            return this.EvaluateScale(this.nodes);
        }
        
        public T FindChild<T>(string name) where T : class, ITimeScaleNode
        {
            for (int i = 0, count = this.nodes.Count; i < count; i++)
            {
                ITimeScaleNode node = this.nodes[i];
                if (node.Name == name)
                {
                    return node as T;
                }
            }

            return null;
        }

        public ITimeScaleNode FindChild(string name)
        {
            for (int i = 0, count = this.nodes.Count; i < count; i++)
            {
                ITimeScaleNode node = this.nodes[i];
                if (node.Name == name)
                {
                    return node;
                }
            }

            return null;
        }
    }
}