using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    public class TypeTreeNode
    {
        public string m_Type;
        public string m_Name;
        public int m_ByteSize;
        public int m_Index;
        public int m_TypeFlags; //m_IsArray
        public int m_Version;
        public int m_MetaFlag;
        public int m_Level;
        public uint m_TypeStrOffset;
        public uint m_NameStrOffset;
        public ulong m_RefTypeHash;

        public TypeTreeNode() { }

        public TypeTreeNode(string type, string name, int level, bool align)
        {
            m_Type = type;
            m_Name = name;
            m_Level = level;
            m_MetaFlag = align ? 0x4000 : 0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"类型: {m_Type} | ");
            sb.Append($"名称: {m_Name} | ");
            sb.Append($"字节大小: 0x{m_ByteSize:X8} | ");
            sb.Append($"索引: {m_Index} | ");
            sb.Append($"类型标志: {m_TypeFlags} | ");
            sb.Append($"版本: {m_Version} | ");
            sb.Append($"类型字符串偏移量: 0x{m_TypeStrOffset:X8} | ");
            sb.Append($"名称字符串偏移量: 0x{m_NameStrOffset:X8}");
            return sb.ToString();
        }
    }
}
