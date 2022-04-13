using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Utilities.Resourcing
{
    public sealed class Resource : IDisposable
    {
        private byte[] _cachedData;

        public string Path;

        public byte[] ReadData()
        {
            if (_cachedData is not null)
                return _cachedData;
            else
            {
                _cachedData = File.ReadAllBytes(Path);
                return _cachedData;
            }
        }

        public void Dispose()
        {
            Array.Clear(_cachedData, 0, _cachedData.Length);
            _cachedData = null;
        }
    }
}
