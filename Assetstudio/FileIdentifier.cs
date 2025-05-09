using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    public class FileIdentifier
    {
        public Guid guid;
        public int type; //enum { kNonAssetType = 0, kDeprecatedCachedAssetType = 1, kSerializedAssetType = 2, kMetaAssetType = 3 };
        public string pathName;

        //custom
        public string fileName;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"指南: {guid} | ");
            sb.Append($"类型: {type} | ");
            sb.Append($"路径名称: {pathName} | ");
            sb.Append($"文件名称: {fileName}");
            return sb.ToString();
        }
    }
}
