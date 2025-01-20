using System;

namespace Modules.Extensions
{
    [Serializable]
    public struct BufferData<T>
    {
        public T[] values;
        public int size;
    }
}