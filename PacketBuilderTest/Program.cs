using SerializedPacketBuilder;

namespace PacketBuilderTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new PacketBuilder();
            builder.Add(1);
            builder.Add((IntPtr)2);
            builder.Add(3.0f);
            builder.Add("Hello, World!");
            builder.Add(new ulong[1000]);

            var bytes = builder.ToBytes();

            var destr = new PacketDestruktor(bytes);

            var first = destr.Deserialize<int>();
            var second = destr.Deserialize<IntPtr>();
            var third = destr.Deserialize<float>();
            var fourth = destr.Deserialize();
            var fifth = destr.DeserializeArray<ulong>();
        }
    }
}