using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Utilities
{
    public static class StreamUtils
    {
        public static byte[] ReadData(this Stream stream, int length)
        {
            byte[] data = new byte[length];
            stream.Read(data, 0, length);
            return data;
        }
        public static byte[] ReadData(this Stream stream, long length)
        {
            byte[] data = new byte[length];
            stream.Read(data, 0, (int)length);
            return data;
        }
        public static Stream ConnectStreamsA<TStream>(this Stream stream1, Stream stream2, int length)
        {
            stream1.Write(stream2.ReadData(length));
            return stream1;
        }
        public static Stream ConnectStreamsB(this Stream stream1, Stream stream2, int length)
        {
            stream2.Write(stream1.ReadData(length));
            return stream2;
        }
    }
}
