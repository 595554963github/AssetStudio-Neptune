using System;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;

namespace AssetStudio
{
    public class UnityCN
    {
        private const string Signature = "#$unity3dchina!@";

        private static ICryptoTransform Encryptor;

        public byte[] Index = new byte[0x10];
        public byte[] Sub = new byte[0x10];

        public UnityCN(EndianBinaryReader reader)
        {
            var infoBytes = reader.ReadBytes(0x8);
            reader.AlignStream();

            infoBytes = infoBytes.ToUInt4Array();
;           Index = Array.Empty<byte>();
            var subBytes = infoBytes.AsSpan(0, 0x10);
            for (var i = 0; i < subBytes.Length; i++)
            {
                var idx = (i % 4 * 4) + (i / 4);
                Sub[idx] = subBytes[i];
            }

        }

        public static bool SetKey(Entry entry)
        {
            Logger.Verbose($"使用密钥初始化解密器{entry.Key}");
            try
            {
                using var aes = Aes.Create();
                aes.Mode = CipherMode.ECB;
                aes.Key = Convert.FromHexString(entry.Key);

                Encryptor = aes.CreateEncryptor();
                Logger.Verbose($"解密器初始化!!");
            }
            catch (Exception e)
            {
                Logger.Error($"[UnityCN]无效的密钥!!\n{e.Message}");
                return false;
            }
            return true;
        }

        public void DecryptBlock(Span<byte> bytes, int size, int index)
        {
            int count = 0;
            var offset = 0;
            while (offset < size)
            {
                if (count++ >= 0x14) break;
                offset += Decrypt(bytes.Slice(offset), index++, size - offset);
            }
        }

        private void DecryptKey(byte[] key, byte[] data)
        {
            if (Encryptor != null)
            {
                key = Encryptor.TransformFinalBlock(key, 0, key.Length);
                for (int i = 0; i < 0x10; i++)
                    data[i] ^= key[i];
            }
        }

        private int DecryptByte(Span<byte> bytes, ref int offset, ref int index)
        {
            var b = Sub[((index >> 2) & 3) + 4] + Sub[index & 3] + Sub[((index >> 4) & 3) + 8] + Sub[((byte)index >> 6) + 12];
            bytes[offset] = byte.RotateLeft(bytes[offset], b & 7);
            b = bytes[offset];
            offset++;
            index++;
            return b;
        }

        private int Decrypt(Span<byte> bytes, int index, int remaining)
        {
            var offset = 0;

            var curByte = DecryptByte(bytes, ref offset, ref index);
            var byteHigh = curByte >> 4;
            var byteLow = curByte & 0xF;

            if (byteHigh == 0xF)
            {
                int b;
                do
                {
                    b = DecryptByte(bytes, ref offset, ref index);
                    byteHigh += b;
                } while (b == 0xFF);
            }

            offset += byteHigh;

            if (offset < remaining)
            {
                DecryptByte(bytes, ref offset, ref index);
                DecryptByte(bytes, ref offset, ref index);
                if (byteLow == 0xF)
                {
                    int b;
                    do
                    {
                        b = DecryptByte(bytes, ref offset, ref index);
                    } while (b == 0xFF);
                }
            }

            return offset;
        }

        public class Entry
        {
            public string Name { get; private set; }
            public string Key { get; private set; }

            public Entry(string name, string key)
            {
                Name = name;
                Key = key;
            }

            public bool Validate()
            {
                var bytes = Convert.FromHexString(Key);
                if (bytes.Length != 0x10)
                {
                    Logger.Warning($"[UnityCN] {this}密钥无效,大小应为16字节，跳过...");
                    return false;
                }

                return true;
            }

            public override string ToString() => $"{Name} ({Key})";
        }
    }
}
