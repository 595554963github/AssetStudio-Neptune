using System;
using System.ComponentModel;
using System.Configuration;

namespace AssetStudio.CLI.Properties {
    public static class AppSettings
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static TValue Get<TValue>(string key, TValue defaultValue)
        {
            try
            {
                var value = Get(key);

                if (string.IsNullOrEmpty(value)) 
                    return defaultValue;

                return (TValue)TypeDescriptor.GetConverter(typeof(TValue)).ConvertFromInvariantString(value);
            }
            catch (Exception)
            {
                Console.WriteLine($"无效值在\"{key}\",切换到默认值[{defaultValue}] !!");
                return defaultValue;
            }
            
        }
    }

    public class Settings
    {
        private static Settings defaultInstance = new Settings();

        public static Settings Default => defaultInstance;

        public bool convertTexture => AppSettings.Get("转换纹理", true);
        public bool convertAudio => AppSettings.Get("转换音频", true);
        public ImageFormat convertType => AppSettings.Get("转换类型", ImageFormat.Png);
        public bool eulerFilter => AppSettings.Get("欧拉过滤器", true);
        public decimal filterPrecision => AppSettings.Get("过滤精度", (decimal)0.25);
        public bool exportAllNodes => AppSettings.Get("导出所有节点", true);
        public bool exportSkins => AppSettings.Get("导出皮肤", true);
        public bool exportMaterials => AppSettings.Get("导出材质", false);
        public bool collectAnimations => AppSettings.Get("收集动画", true);
        public bool exportAnimations => AppSettings.Get("导出动画", true);
        public decimal boneSize => AppSettings.Get("骨骼大小", (decimal)10);
        public int fbxVersion => AppSettings.Get("fbx版本", 3);
        public int fbxFormat => AppSettings.Get("fbx格式", 0);
        public decimal scaleFactor => AppSettings.Get("缩放因子", (decimal)1);
        public bool exportBlendShape => AppSettings.Get("导出混合形状", true);
        public bool castToBone => AppSettings.Get("投射到骨骼", false);
        public bool restoreExtensionName => AppSettings.Get("恢复文件扩展名", true);
        public bool enableFileLogging => AppSettings.Get("启用文件日志记录", false);
        public bool minimalAssetMap => AppSettings.Get("最小资源映射", true);
        public bool allowDuplicates => AppSettings.Get("允许重复", false);
        public string types => AppSettings.Get("类型", string.Empty);
        public string texs => AppSettings.Get("文本", string.Empty);
        public string uvs => AppSettings.Get("uv坐标", string.Empty);

    }
}
