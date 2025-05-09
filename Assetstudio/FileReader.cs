using System;
using System.IO;
using System.Linq;
using static AssetStudio.ImportHelper;

namespace AssetStudio
{
    public class FileReader : EndianBinaryReader
    {
        public string FullPath;
        public string FileName;
        public FileType FileType;

        private static readonly byte[] gzipMagic = new byte[] { 31, 139 };
        private static readonly byte[] brotliMagic = new byte[] { 98, 114, 111, 116, 108, 105 };
        private static readonly byte[] zipMagic = new byte[] { 80, 75, 3, 4 };
        private static readonly byte[] zipSpannedMagic = new byte[] { 80, 75, 7, 8 };
        private static readonly byte[] mhy0Magic = new byte[] { 109, 104, 121, 48 };
        private static readonly byte[] mhy1Magic = new byte[] { 109, 104, 121, 49 };
        private static readonly byte[] blb2Magic = new byte[] { 66, 108, 98, 2 };
        private static readonly byte[] blb3Magic = new byte[] { 66, 108, 98, 3 };
        private static readonly byte[] narakaMagic = new byte[] { 21, 30, 28, 13, 13, 35, 33 };
        private static readonly byte[] gunfireMagic = new byte[] { 124, 109, 121, 114, 39, 122, 115, 120, 63 };


        public FileReader(string path) : this(path, File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) { }

        public FileReader(string path, Stream stream, bool leaveOpen = false) : base(stream, EndianType.BigEndian, leaveOpen)
        {
            FullPath = Path.GetFullPath(path);
            FileName = Path.GetFileName(path);
            FileType = CheckFileType();
            Logger.Verbose($"文件{path}类型是{FileType}");
        }

        private FileType CheckFileType()
        {
            var signature = this.ReadStringToNull(20);
            Position = 0;
            Logger.Verbose($"解析签名是{signature}");
            switch (signature)
            {
                case "UnityWeb":
                case "UnityRaw":
                case "UnityArchive":
                case "UnityFS":
                    return FileType.BundleFile;
                case "UnityWebData1.0":
                    return FileType.WebFile;
                case "blk":
                    return FileType.BlkFile;
                case "ENCR":
                    return FileType.ENCRFile;
                default:
                    {
                        Logger.Verbose("签名与任何支持的字符串签名都不匹配,尝试检查字节签名");
                        byte[] magic = ReadBytes(2);
                        Position = 0;
                        Logger.Verbose($"解析签名是{Convert.ToHexString(magic)}");
                        if (gzipMagic.SequenceEqual(magic))
                        {
                            return FileType.GZipFile;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(gzipMagic)}");
                        Position = 0x20;
                        magic = ReadBytes(6);
                        Position = 0;
                        Logger.Verbose($"解析签名是{Convert.ToHexString(magic)}");
                        if (brotliMagic.SequenceEqual(magic))
                        {
                            return FileType.BrotliFile;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(brotliMagic)}");
                        if (IsSerializedFile())
                        {
                            return FileType.AssetsFile;
                        }
                        magic = ReadBytes(4);
                        Position = 0;
                        Logger.Verbose($"解析签名是{Convert.ToHexString(magic)}");
                        if (zipMagic.SequenceEqual(magic) || zipSpannedMagic.SequenceEqual(magic))
                        {
                            return FileType.ZipFile;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(zipMagic)} or {Convert.ToHexString(zipSpannedMagic)}");
                        if (mhy0Magic.SequenceEqual(magic) || mhy1Magic.SequenceEqual(magic))
                        {
                            return FileType.MhyFile;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(mhy0Magic)} or {Convert.ToHexString(mhy1Magic)}");
                        if (blb2Magic.SequenceEqual(magic))
                        {
                            return FileType.Blb2File;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(blb2Magic)}");
                        if (blb3Magic.SequenceEqual(magic))
                        {
                            return FileType.Blb3File;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(blb3Magic)}");
                        magic = ReadBytes(7);
                        Position = 0;
                        Logger.Verbose($"解析签名是{Convert.ToHexString(magic)}");
                        if (narakaMagic.SequenceEqual(magic))
                        {
                            return FileType.BundleFile;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(narakaMagic)}");
                        magic = ReadBytes(9);
                        Position = 0;
                        Logger.Verbose($"解析签名是{Convert.ToHexString(magic)}");
                        if (gunfireMagic.SequenceEqual(magic))
                        {
                            Position = 0x32;
                            return FileType.BundleFile;
                        }
                        Logger.Verbose($"解析的签名与预期的签名不匹配{Convert.ToHexString(gunfireMagic)}");
                        Logger.Verbose($"解析的签名与任何支持的签名都不匹配,假设是资源文件");
                        return FileType.ResourceFile;
                    }
            }
        }


        private bool IsSerializedFile()
        {
            Logger.Verbose($"尝试检查文件是否是序列化文件...");

            var fileSize = BaseStream.Length;
            if (fileSize < 20)
            {
                Logger.Verbose($"文件大小0x{fileSize:X8}太小了,最小可接受尺寸为0x14,终止操作...");
                return false;
            }
            var m_MetadataSize = ReadUInt32();
            long m_FileSize = ReadUInt32();
            var m_Version = ReadUInt32();
            long m_DataOffset = ReadUInt32();
            var m_Endianess = ReadByte();
            var m_Reserved = ReadBytes(3);
            if (m_Version >= 22)
            {
                if (fileSize < 48)
                {
                    Logger.Verbose($"文件大小0x{fileSize:X8}对于版本{m_Version}太小了,最小可接受尺寸为0x30,终止操作...");
                    Position = 0;
                    return false;
                }
                m_MetadataSize = ReadUInt32();
                m_FileSize = ReadInt64();
                m_DataOffset = ReadInt64();
            }
            Position = 0;
            if (m_FileSize != fileSize)
            {
                Logger.Verbose($"解析文件大小0x{m_FileSize:X8}与流大小不匹配{fileSize},文件可能已损坏,终止操作...");
                return false;
            }
            if (m_DataOffset > fileSize)
            {
                Logger.Verbose($"解析数据偏移量0x{m_DataOffset:X8}在流的大小之外{fileSize},文件可能已损坏,终止操作...");
                return false;
            }
            Logger.Verbose($"有效的序列化文件!!");
            return true;
        }
    }

    public static class FileReaderExtensions
    {
        public static FileReader PreProcessing(this FileReader reader, Game game)
        {
            Logger.Verbose($"对文件应用预处理{reader.FileName}");
            if (reader.FileType == FileType.ResourceFile || !game.Type.IsNormal())
            {
                Logger.Verbose("文件已加密!!");
                switch (game.Type)
                {
                    case GameType.GI_Pack:
                        reader = DecryptPack(reader, game);
                        break;
                    case GameType.GI_CB1:
                        reader = DecryptMark(reader);
                        break;
                    case GameType.偶像梦幻祭2:
                        reader = DecryptEnsembleStar(reader);
                        break;
                    case GameType.航海王热血航线:
                    case GameType.FakeHeader:                   
                    case GameType.胜利女神妮姬:
                        reader = ParseFakeHeader(reader);
                        break;                                         
                    case GameType.风之幻想:
                        reader = DecryptFantasyOfWind(reader);
                        break;
                    case GameType.螺旋圆舞曲2蔷薇战争:
                        reader = ParseHelixWaltz2(reader);
                        break;
                    case GameType.锚点降临:
                        reader = DecryptAnchorPanic(reader);
                        break;
                    case GameType.梦间集天鹅座:
                        reader = DecryptDreamscapeAlbireo(reader);
                        break;
                    case GameType.魔法禁书目录幻想收束:
                        reader = DecryptImaginaryFest(reader);
                        break;
                    case GameType.机甲爱丽丝:
                        reader = DecryptAliceGearAegis(reader);
                        break;
                    case GameType.世界计划多彩舞台:
                        reader = DecryptProjectSekai(reader);
                        break;
                    case GameType.jump群星集结:
                        reader = DecryptCodenameJump(reader);
                        break;
                    case GameType.少女前线:
                        reader = DecryptGirlsFrontline(reader);
                        break; 
                    case GameType.重返未来1999:
                        reader = DecryptReverse1999(reader);
                        break;
                    case GameType.咒术回战幻影夜行:
                        reader = DecryptJJKPhantomParade(reader);
                        break;
                    case GameType.MuvLuv维度:
                        reader = DecryptMuvLuvDimensions(reader);
                        break;
                    case GameType.动物派对:
                        reader = DecryptPartyAnimals(reader);
                        break;
                    case GameType.恋与深空:
                        reader = DecryptLoveAndDeepspace(reader);
                        break;
                    case GameType.学园少女突袭者:
                        reader = DecryptSchoolGirlStrikers(reader);
                        break;
                    case GameType.物华弥新:
                        reader = DecryptPerpetualNovelty(reader);
                        break;
                    case GameType.无期迷途:
                        reader = Decryptwuqimitu(reader);
                        break;
                }
            }
            if (reader.FileType == FileType.BundleFile && game.Type.IsBlockFile() || reader.FileType == FileType.ENCRFile || reader.FileType == FileType.Blb2File || reader.FileType == FileType.Blb3File)
            {
                Logger.Verbose("文件可能有多个包!!");
                try
                {
                    var signature = reader.ReadStringToNull();
                    reader.ReadInt32();
                    reader.ReadStringToNull();
                    reader.ReadStringToNull();
                    var size = reader.ReadInt64();
                    if (size != reader.BaseStream.Length)
                    {
                        Logger.Verbose($"找到签名{signature},预期捆绑包大小为0x{size:X8},找到0x{reader.BaseStream.Length}取代原操作!!");
                        Logger.Verbose("作为块文件加载!!");
                        reader.FileType = FileType.BlockFile;
                    }
                }
                catch (Exception) { }
                reader.Position = 0;
            }

            if (reader.FileType == FileType.MhyFile && (game.Type.IsZZZCB2() || game.Type.IsZZZ()))
            {
                reader.FileType = FileType.BlockFile;
            }
            Logger.Verbose("无需预处理");
            return reader;
        }
    } 
}
