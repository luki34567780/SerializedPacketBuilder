using SerializedPacketBuilder;

namespace SharedData
{
    public class FilePacket : ISerializable<FilePacket>
    {
        public FilePacket() { }

        public FilePacket(string fileName, byte[] data)
        {
            FileName = fileName;
            Data = data;
        }

        public FilePacket(string fileName, string filePath)
        {
            FileName = fileName;
            Data = File.ReadAllBytes(filePath);
        }

        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public void Load(PacketDestruktor destr)
        {
            FileName = destr.Deserialize();
            Data = destr.DeserializeArray<byte>();
        }

        public void Serialize(PacketBuilder builder)
        {
            builder.Add(FileName);
            builder.Add(Data);
        }
    }
}