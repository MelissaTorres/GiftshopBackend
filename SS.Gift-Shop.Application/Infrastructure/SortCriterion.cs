using System.ComponentModel;
using System.Diagnostics;

namespace SS.GiftShop.Application.Infrastructure
{
    [DebuggerDisplay("{Name,nq} {Direction}")]
    public sealed class SortCriterion
    {
        public string Name { get; }

        public ListSortDirection Direction { get; }

        public SortCriterion(string name, ListSortDirection direction)
        {
            Name = name;
            Direction = direction;
        }
    }
}
