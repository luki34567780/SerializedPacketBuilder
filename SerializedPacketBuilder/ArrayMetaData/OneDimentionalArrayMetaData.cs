using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializedPacketBuilder.ArrayMetaData
{
    internal struct OneDimentionalArrayMetaData
    {
        public int CountX;

        public T[] GetArray<T>() where T : unmanaged => new T[CountX];
    }
}
