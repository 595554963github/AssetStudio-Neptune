using System.Text;

namespace AssetStudio
{
    public class ObjectInfo
    {
        public long byteStart;
        public uint byteSize;
        public int typeID;
        public int classID;
        public ushort isDestroyed;
        public byte stripped;

        public long m_PathID;
        public SerializedType serializedType;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"字节起始: 0x{byteStart:X8} | ");
            sb.Append($"字节大小: 0x{byteSize:X8} | ");
            sb.Append($"类型ID: {typeID} | ");
            sb.Append($"类ID: {classID} | ");
            sb.Append($"是否已销毁: {isDestroyed} | ");
            sb.Append($"是否已剥离: {stripped} | ");
            sb.Append($"路径ID: {m_PathID}");
            return sb.ToString();
        }
    }
}
