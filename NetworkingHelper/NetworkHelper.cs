using System.Net.Sockets;

namespace NetworkingHelper
{
    public class NetworkHelper
    {
        private byte[] buffer = new byte[1024];
        public TcpClient Client { get; set; }

        public NetworkStream Stream { get; set; }

        public NetworkHelper(TcpClient client)
        {
            Client = client;
            Stream = client.GetStream();
        }

        public async Task<Memory<byte>> ReceiveBytes(int count)
        {
            if (count > buffer.Length)
            {
                buffer = new byte[count];
            }

            await Stream.ReadExactlyAsync(buffer, 0, count);

            return buffer.AsMemory(0, count);
        }

        public async Task SendBytes(ReadOnlyMemory<byte> bytes)
        {
            await Stream.WriteAsync(bytes);
        }
    }
}