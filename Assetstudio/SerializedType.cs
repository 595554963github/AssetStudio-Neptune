using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    public class SerializedType
    {
        public int classID;
        public bool m_IsStrippedType;
        public short m_ScriptTypeIndex = -1;
        public TypeTree m_Type;
        public byte[] m_ScriptID; //Hash128
        public byte[] m_OldTypeHash; //Hash128
        public int[] m_TypeDependencies;
        public string m_KlassName;
        public string m_NameSpace;
        public string m_AsmName;

        public bool Match(params string[] hashes) => hashes.Any(x => x == Convert.ToHexString(m_OldTypeHash));
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"类ID: {classID} | ");
            sb.Append($"类的类型: {m_IsStrippedType} | ");
            sb.Append($"脚本类型索引: {m_ScriptTypeIndex} | ");
            sb.Append($"类名称: {m_KlassName} | ");
            sb.Append($"命名空间: {m_NameSpace} | ");
            sb.Append($"程序集名称: {m_AsmName}");
            return sb.ToString();
        }
    }
}
