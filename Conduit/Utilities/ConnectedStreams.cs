using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Utilities
{
    internal class ConnectedStreams : Stream
    {
        public override bool CanRead => true;

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => true;

        public override long Length => LengthBS1 + LengthBS2;

        public override long Position { get; set; }

        public Stream BaseStream1;
        public long LengthBS1;
        public bool IsSeek1;

        public Stream BaseStream2;
        public long LengthBS2;
        public bool IsSeek2;

        public ConnectedStreams(Stream baseStream1, Stream baseStream2)
        {
            BaseStream1 = baseStream1;
            LengthBS1 = BaseStream1.Length;
            BaseStream2 = baseStream2;
            LengthBS2 = BaseStream2.Length;
        }
        public ConnectedStreams(Stream baseStream1, long l1, Stream baseStream2, long l2)
        {
            BaseStream1 = baseStream1;
            LengthBS1 = l1;
            BaseStream2 = baseStream2;
            LengthBS2 = l2;
        }
        public ConnectedStreams(Stream baseStream1, Stream baseStream2, long l2)
        {
            BaseStream1 = baseStream1;
            LengthBS1 = BaseStream1.Length;
            BaseStream2 = baseStream2;
            LengthBS2 = l2;
        }
        public ConnectedStreams(Stream baseStream1, long l1, Stream baseStream2)
        {
            BaseStream1 = baseStream1;
            LengthBS1 = l1;
            BaseStream2 = baseStream2;
            LengthBS2 = BaseStream2.Length;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Position < LengthBS2 && Position + count > LengthBS1)
            {
                if (IsSeek2)
                    BaseStream2.Position = Position - count;
                return BaseStream2.Read(buffer, offset, count);
            }
            else if (Position < LengthBS1 && Position + count < LengthBS1)
            {
                if (IsSeek2)
                    BaseStream1.Position = Position;
                return BaseStream1.Read(buffer, offset, count);
            }
            else if (Position < LengthBS1 && Position + count > LengthBS1)
            {
                BaseStream1.Position = Position;
                BaseStream2.Position = 0;

                byte[] buf1 = new byte[LengthBS1 - Position];
                byte[] buf2 = new byte[LengthBS2 - (Position + count)];

                int readed;

                readed = BaseStream1.Read(buf1, 0, buf1.Length);
                readed += BaseStream2.Read(buf2, 0, buf2.Length);

                Array.Copy(buf1, buffer, buf1.LongLength);
                Array.Copy(buf2, 0, buffer, buf1.LongLength, buf2.LongLength);

                return readed;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            
        }
    }
}
