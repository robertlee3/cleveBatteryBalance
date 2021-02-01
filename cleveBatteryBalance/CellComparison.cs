using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace cleveBatteryBalance
{
    internal class CellComparison : IComparer<BatteryCell>
    {
        public int Compare([AllowNull] BatteryCell x, [AllowNull] BatteryCell y)
        {
            if(x.TotalValue > y.TotalValue)
            {
                return 1;
            }else if(x.TotalValue < y.TotalValue)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}