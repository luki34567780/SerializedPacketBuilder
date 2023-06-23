using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SerializedPacketBuilder.ArrayMetaData
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TwoDimentionalArrayMetaData
    {
        public int CountX;
        public int CountY;

        public T[,] GetArray<T>() where T: unmanaged => new T[CountX, CountY];
    }
}
