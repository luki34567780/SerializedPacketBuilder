using SerializedPacketBuilder;

namespace PacketBuilderTest
{
    internal class Program
    {
        private struct TestData : ISerializable<TestData>
        {
            public int Val;
            public string Str;
            public void Load(PacketDestruktor destr)
            {
                Val = destr.Deserialize<int>();
                Str = destr.Deserialize();
            }

            public void Serialize(PacketBuilder builder)
            {
                builder.Add(Val);
                builder.Add(Str);
            }
        }
        static unsafe void Main(string[] args)
        {
            var test = stackalloc byte[1024*1024*1024];


            var builder = new PacketBuilder();
            builder.Add(1);
            builder.Add((IntPtr)2);
            builder.Add(3.0f);
            builder.Add("Hello, World!");
            builder.Add(new ulong[1000]);
            builder.Add(new TestData() { Val = 5, Str = "Test" });

            var bytes = builder.ToBytes();

            var destr = new PacketDestruktor(bytes);

            var first = destr.Deserialize<int>();
            var second = destr.Deserialize<IntPtr>();
            var third = destr.Deserialize<float>();
            var fourth = destr.Deserialize();
            var fifth = destr.DeserializeArray<ulong>();
            var sixth = destr.DeserializeStruct<TestData>();
        }
    }
}