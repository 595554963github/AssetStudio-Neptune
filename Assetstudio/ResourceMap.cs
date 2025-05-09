using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AssetStudio
{
    public static class ResourceMap
    {
        private static AssetMap Instance = new() { GameType = GameType.正常, AssetEntries = new List<AssetEntry>() };
        public static List<AssetEntry> GetEntries() => Instance.AssetEntries;
        public static GameType GetGameType() => Instance.GameType;
        public static int FromFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Logger.Info(string.Format("正在解析...."));
                try
                {
                    var extension = Path.GetExtension(path).ToLower();
                    using var stream = File.OpenRead(path);

                    if (extension == ".map")
                    {
                        // Deserialize map
                        Instance = MessagePackSerializer.Deserialize<AssetMap>(stream, MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
                    }
                    else if (extension == ".json")
                    {
                        // Deserialize json
                        using var reader = new StreamReader(stream);
                        var jsonContent = reader.ReadToEnd();
                        var parsed = JsonConvert.DeserializeObject<AssetMap>(jsonContent);

                        Instance = new AssetMap
                        {
                            GameType = parsed.GameType,
                            AssetEntries = parsed.AssetEntries
                        };
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("资源映射未能加载");
                    Console.WriteLine(e.ToString());
                    return -1;
                }
                Logger.Info("已加载!!");
                return 1;
            }
            else
            {
                Logger.Error("资源映射未能加载");
                return -1;
            }
        }

        public static void Clear()
        {
            Instance.GameType = GameType.正常;
            Instance.AssetEntries = new List<AssetEntry>();
        }
    }
}
