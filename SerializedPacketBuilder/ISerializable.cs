using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializedPacketBuilder
{
    public interface ISerializable<T> where T: new()
    {
        public void Serialize(PacketBuilder builder);

        public void Load(PacketDestruktor destr);
    }
}
