using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SerializedPacketBuilder.ArrayMetaData;

namespace SerializedPacketBuilder
{
    public unsafe class PacketDestruktor : IDisposable
    {
        private byte[] _buffer;
        private GCHandle _bufferHandle;
        private byte* _bufferPtr;
        private int _offset;
        private int _size;
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public PacketDestruktor(Span<byte> data)
        {
            LoadData(data);
        }

        public T[,,] DeserializeArray3<T>() where T : unmanaged
        {
            var metaData = Deserialize<ThreeDimentionalArrayMetaData>();
            var items = metaData.GetArray<T>();

            for (var x = 0; x < metaData.CountX; x++)
                for (var y = 0; y < metaData.CountY; y++)
                    for (var z = 0; z < metaData.CountZ; z++)
                        items[x, y, z] = Deserialize<T>();

            return items;
        }

        public T[,] DeserializeArray2<T>() where T : unmanaged
        {
            var metaData = Deserialize<TwoDimentionalArrayMetaData>();
            var items = metaData.GetArray<T>();

            for (var x = 0; x < metaData.CountX; x++)
                for (var y = 0; y < metaData.CountY; y++)
                    items[x, y] = Deserialize<T>();

            return items;
        }

        public T[] DeserializeArray<T>() where T : unmanaged
        {
            var metaData = Deserialize<OneDimentionalArrayMetaData>();

            var items = metaData.GetArray<T>();

            for (var i = 0; i < metaData.CountX; i++)
                items[i] = Deserialize<T>();

            return items;
        }

        public void LoadData(Span<byte> data)
        {
            _buffer = data.ToArray();
            _bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
            _bufferPtr = (byte*)_bufferHandle.AddrOfPinnedObject();
            _offset = 0;
            _size = data.Length;
        }

        public T DeserializeStruct<T>() where T : ISerializable<T>, new()
        {
            var instance = new T();
            instance.Load(this);
            return instance;
        }

        public Array DeserializeArrayN<T>() where T : unmanaged
        {
            var metaData = Deserialize<ManyDimentionalArrayMetaData>();
            var items = metaData.GetArray<T>();

            for (int i = 0; i < metaData.DimentionCount; i++)
            {
                int dimentionSize = metaData.Dimentions[i];
                for (int j = 0; j < dimentionSize; j++)
                {
                    items.SetValue(Deserialize<T>(), j);
                }
            }

            return items;
        }

        public string Deserialize()
        {
            return Encoding.GetString(DeserializeArray<byte>());
        }

        public T Deserialize<T>() where T : unmanaged
        {
            var size = Marshal.SizeOf<T>();

            if (_offset + size > _size)
                throw new Exception("Out of Range!");

            var item = *(T*)(_bufferPtr + _offset);

            _offset += size;

            return item;
        }

        public void Dispose()
        {
            _bufferPtr = (byte*)IntPtr.Zero;
            _bufferHandle.Free();
        }
    }
}
