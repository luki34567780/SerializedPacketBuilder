using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializedPacketBuilder.ArrayMetaData
{
    internal struct ThreeDimentionalArrayMetaData
    {
        public int CountX;
        public int CountY;
        public int CountZ;

        public T[,,] GetArray<T>() where T : unmanaged => new T[CountX, CountY, CountZ];
    }
}
