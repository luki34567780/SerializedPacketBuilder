using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public T[] DeserializeArray<T>() where T : unmanaged
        {
            var count = Deserialize<int>();
            var items = new T[count];

            for (var i = 0; i < count; i++)
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
