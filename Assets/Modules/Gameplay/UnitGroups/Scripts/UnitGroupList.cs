using Atomic.Elements;
using Modules.Gameplay;

namespace Game.Gameplay
{
    public sealed class UnitGroupList : ReactiveList<UnitGroup>
    {
        public UnitGroupList(params UnitGroup[] items) : base(items)
        {
        }

        public UnitGroupList(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Add(new UnitGroup());
            }
        }
    }
}