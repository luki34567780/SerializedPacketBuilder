using SerializedPacketBuilder.ArrayMetaData;
using System.Runtime.InteropServices;
using System.Text;

namespace SerializedPacketBuilder
{
    public unsafe class PacketBuilder
    {
        private byte* _buffer = (byte*)NativeMemory.Alloc(128);
        private int _offset = 0;
        private int _maxSize = 128;
        public int Size => _offset;
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public void Add(string str)
        {
            var bytes = Encoding.GetBytes(str);

            // add bytes
            // Add(T[]) will automaticly add size of array
            Add(bytes);
        }

        public void Add<T>(ISerializable<T> serializable) where T: new()
        {
            serializable.Serialize(this);
        }

        public void Add<T>(T[] items) where T: unmanaged
        {
            Add(new OneDimentionalArrayMetaData() { CountX = items.Length });

            foreach(var item in items)
                Add(item);
        }

        public void Add<T>(T[,] items) where T: unmanaged
        {
            var metaData = new TwoDimentionalArrayMetaData() { CountX = items.GetLength(0), CountY = items.GetLength(1) };
            Add(metaData);

            for (int x = 0; x < metaData.CountX; x++)
                for (int y = 0; y < metaData.CountY; y++)
                    Add(items[x, y]);
        }

        public void Add<T>(T[,,] items) where T : unmanaged
        {
            var metaData = new ThreeDimentionalArrayMetaData() { CountX = items.GetLength(0), CountY = items.GetLength(1), CountZ = items.GetLength(2) };
            Add(metaData);

            for (int x = 0; x < metaData.CountX; x++)
                for (int y = 0; y < metaData.CountY; y++)
                    for (int z = 0; z < metaData.CountZ; z++)
                        Add(items[x, y, z]);
        }

        public void Add<T>(T item) where T: unmanaged
        {
            var size = Marshal.SizeOf(item);

            // reallocate buffer if needed
            if (_offset + size > _maxSize)
            {
                while (_offset + size > _maxSize)
                    _maxSize *= 2;

                var buf = (byte*)NativeMemory.Realloc(_buffer, (nuint)_maxSize);
                //NativeMemory.Free(_buffer);
                _buffer = buf;
            }

            NativeMemory.Copy(&item, _buffer + _offset, (nuint)size);

            _offset += size;
        }

        public byte[] ToBytes()
        {
            var buf = new byte[Size];
            ToBuffer(buf);
            return buf;
        }

        public void ToBuffer(Span<byte> data)
        {
            if (Size > data.Length)
                throw new System.Exception("Buffer is too small");

            fixed(byte* dataPtr = &data[0])
            {
                NativeMemory.Copy(_buffer, dataPtr, (nuint)Size);
            }
        }
    }
}