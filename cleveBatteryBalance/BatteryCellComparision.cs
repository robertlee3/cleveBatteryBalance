using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace cleveBatteryBalance
{
    internal class BatteryCellComparision : IComparer<IndexedValue>
    {
        public int Compare([AllowNull] IndexedValue x, [AllowNull] IndexedValue y)
        {
            if(x.Value < y.Value)
            {
                return -1;
            }else if (x.Value > y.Value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}