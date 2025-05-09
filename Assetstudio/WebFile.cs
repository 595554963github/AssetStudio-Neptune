using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AssetStudio
{
    public class WebFile
    {
        public List<StreamFile> fileList;

        private class WebData
        {
            public int dataOffset;
            public int dataLength;
            public string path;

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append($"数据偏移量: 0x{dataOffset:X8} | ");
                sb.Append($"数据偏移量: 0x{dataLength:X8} | ");
                sb.Append($"路径: {path}");
                return sb.ToString();
            }
        }

        public WebFile(EndianBinaryReader reader)
        {
            reader.Endian = EndianType.LittleEndian;
            var signature = reader.ReadStringToNull();
            var headLength = reader.ReadInt32();
            var dataList = new List<WebData>();
            Logger.Verbose($"标头大小: 0x{headLength:X8}");
            while (reader.BaseStream.Position < headLength)
            {
                var data = new WebData();
                data.dataOffset = reader.ReadInt32();
                data.dataLength = reader.ReadInt32();
                var pathLength = reader.ReadInt32();
                Logger.Verbose($"路径长度: {pathLength}");
                data.path = Encoding.UTF8.GetString(reader.ReadBytes(pathLength));
                Logger.Verbose($"Web数据信息: {data}");
                dataList.Add(data);
            }
            Logger.Verbose("将文件写入流...");
            fileList = new List<StreamFile>();
            for (int i = 0; i < dataList.Count; i++)
            {
                var data = dataList[i];
                var file = new StreamFile();
                file.path = data.path;
                file.fileName = Path.GetFileName(data.path);
                reader.BaseStream.Position = data.dataOffset;
                file.stream = new MemoryStream(reader.ReadBytes(data.dataLength));
                fileList[i] = file;
            }
        }
    }
}
