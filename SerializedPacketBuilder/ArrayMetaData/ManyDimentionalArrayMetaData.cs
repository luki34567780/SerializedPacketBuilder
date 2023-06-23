using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SerializedPacketBuilder.ArrayMetaData
{
    internal unsafe struct ManyDimentionalArrayMetaData
    {
        public int DimentionCount;
        public fixed int Dimentions[256];

        public Array GetArray<T>()
        {
            var dimentions = new int[DimentionCount];
            for (int i = 0; i < DimentionCount; i++)
            {
                dimentions[i] = Dimentions[i];
            }

            var arr = Array.CreateInstance(typeof(T), dimentions);

            return arr;
        }
    }
}
