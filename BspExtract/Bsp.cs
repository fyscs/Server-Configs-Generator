using System;
using System.IO;
using System.Threading.Tasks;

namespace BspExtract
{
    public class Bsp : IDisposable
    {
        public struct lump_t
        {
            public int Offset { get; set; }
            public int Length { get; set; }
            public int Version { get; set; }
            public int Identity { get; set; }
        }

        const int VBSP_MAGIC_HEADER = 0x50534256;

        private FileStream File;
        private lump_t[] Lumps;

        public Bsp(string path)
        {
            File = new FileStream(path, FileMode.Open);
            ReadHeader();
        }

        private void ReadHeader()
        {
            var identity = ReadInt32();
            var version = ReadInt32();

            if (identity != VBSP_MAGIC_HEADER)
                throw new Exception($"Bsp header identity error: {identity}");

            //Header = new Dictionary<string, long>();

            Lumps = new lump_t[64];

            for (var i = 0; i < 64; i++)
            {
                var _offset = ReadInt32();
                var _length = ReadInt32();
                var _version = ReadInt32();
                var _identity = ReadInt32();

                Lumps[i] = new lump_t
                {
                    Offset = _offset,
                    Length = _length,
                    Version = _version,
                    Identity = _identity
                };
            }

            var revision = ReadInt32();

            Console.WriteLine($"Read identity={identity} version={version} revision={revision}");
        }

        private int ReadInt32()
        {
            var bytes = new byte[4];
            File.Read(bytes, 0, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        private int ReadInt16()
        {
            var bytes = new byte[2];
            File.Read(bytes, 0, 2);
            return BitConverter.ToInt16(bytes, 0);
        }

        public void Dispose()
        {
            File.Dispose();
        }

        public async Task<byte[]> ReadLumps(int lump)
        {
            if (lump < 0 || lump >= Lumps.Length)
                throw new ArgumentException($"Invalid lump {lump}");

            var bytes = new byte[Lumps[lump].Length - 1]; // 1 byte = null terminator

            File.Seek(Lumps[lump].Offset, SeekOrigin.Begin);
            await File.ReadAsync(bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
