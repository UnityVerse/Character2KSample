using System;
using Sirenix.OdinInspector;

namespace Modules.Gameplay
{
    public sealed class Cell<TKey>
    {
        public event Action<int> OnStateChanged;

        public TKey Key
        {
            get { return _key; }
        }

        [ShowInInspector]
        public int Count
        {
            get { return _count; }
            internal set
            {
                if (_count != value)
                {
                    _count = value;
                    this.OnStateChanged?.Invoke(_count);
                }
            }
        }

        private readonly TKey _key;
        private int _count;

        internal Cell(TKey key, int count)
        {
            _key = key;
            _count = count;
        }

        internal Cell(Cell<TKey> cell)
        {
            _key = cell._key;
            _count = cell._count;
        }
    }
}