using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    public class SerializedFileHeader
    {
        public uint m_MetadataSize;
        public long m_FileSize;
        public SerializedFileFormatVersion m_Version;
        public long m_DataOffset;
        public byte m_Endianess;
        public byte[] m_Reserved;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"元数据大小: 0x{m_MetadataSize:X8} | ");
            sb.Append($"文件大小: 0x{m_FileSize:X8} | ");
            sb.Append($"版本: {m_Version} | ");
            sb.Append($"数据偏移量: 0x{m_DataOffset:X8} | ");
            sb.Append($"字节序: {(EndianType)m_Endianess}");
            return sb.ToString();
        }
    }
}
