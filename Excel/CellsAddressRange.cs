using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel
{
    public struct CellsAddressRange
    {
        public readonly int FromRow;
        public readonly int FromCol;
        public readonly int ToRow;
        public readonly int ToCol;

        public CellsAddressRange(int fromRow,int fromCol,int toRow,int toCol)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
        }
       

    }
}
