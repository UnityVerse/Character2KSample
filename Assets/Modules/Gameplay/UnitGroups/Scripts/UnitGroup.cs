using System;
using System.Collections.Generic;
using Atomic.Elements;
using Atomic.Entities;

namespace Modules.Gameplay
{
    [Serializable]
    public sealed class UnitGroup : ReactiveHashSet<IEntity>
    {
        public UnitGroup()
        {
        }

        public UnitGroup(params IEntity[] elements) : base(elements)
        {
        }

        public UnitGroup(IEnumerable<IEntity> elements) : base(elements)
        {
        }
    }
}