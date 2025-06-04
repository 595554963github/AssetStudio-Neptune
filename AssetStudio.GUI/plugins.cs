using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using super_toolbox;
using UniversalByteRemover;
using UniversalFileExtractor;
using QuickBMSBatchExtractor;

namespace AssetStudio.GUI
{
    public partial class Plugins
    {
        public static ToolStripMenuItem CreateDefaultExtractorMenuItem()
        {
            var menuItem = new ToolStripMenuItem();
            menuItem.Name = "toolStripMenuItem21";
            menuItem.Size = new System.Drawing.Size(180, 22);
            menuItem.Text = "万能二进制提取器";
            menuItem.Visible = true;
            menuItem.Click += 万能二进制提取器ToolStripMenuItem_Click;
            return menuItem;
        }

        public static ToolStripMenuItem CreateDefaultRemoverMenuItem()
        {
            var menuItem = new ToolStripMenuItem();
            menuItem.Name = "toolStripMenuItem22";
            menuItem.Size = new System.Drawing.Size(180, 22);
            menuItem.Text = "万能字节移除器";
            menuItem.Visible = true;
            menuItem.Click += 万能字节移除器ToolStripMenuItem_Click;
            return menuItem;
        }

        public static ToolStripMenuItem CreateSofdec2ViewerMenuItem()
        {
            var menuItem = new ToolStripMenuItem();
            menuItem.Name = "toolStripMenuItem23";
            menuItem.Size = new System.Drawing.Size(180, 22);
            menuItem.Text = "USM视频查看工具汉化版";
            menuItem.Visible = true;
            menuItem.Click += Sofdec2ViewerToolStripMenuItem_Click;
            return menuItem;
        }

        public static ToolStripMenuItem CreateRadVideoMenuItem()
        {
            var menuItem = new ToolStripMenuItem();
            menuItem.Name = "toolStripMenuItem24";
            menuItem.Size = new System.Drawing.Size(180, 22);
            menuItem.Text = "radvideo64.exe";
            menuItem.Visible = true;
            menuItem.Click += RadVideoToolStripMenuItem_Click;
            return menuItem;
        }

        public static ToolStripMenuItem CreateQuickBmsMenuItem()
        {
            var menuItem = new ToolStripMenuItem();
            menuItem.Name = "toolStripMenuItem25";
            menuItem.Size = new System.Drawing.Size(180, 22);
            menuItem.Text = "quickbmsbatch";
            menuItem.Visible = true;
            menuItem.Click += QuickBmsBatchToolStripMenuItem_Click;
            return menuItem;
        }

        public static ToolStripMenuItem CreateSuperToolboxMenuItem()
        {
            var menuItem = new ToolStripMenuItem();
            menuItem.Name = "toolStripMenuItem26";
            menuItem.Size = new System.Drawing.Size(180, 22);
            menuItem.Text = "超级工具箱";
            menuItem.Visible = true;
            menuItem.Click += SuperToolboxToolStripMenuItem_Click;
            return menuItem;
        }

        private static void 万能二进制提取器ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var formFromExtractor = new FileExtractor();
            formFromExtractor.Show();
        }

        private static void 万能字节移除器ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var formFromByteRemover = new ByteRemover();
            formFromByteRemover.Show();
        }

        private static void SuperToolboxToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var formFromSuperToolbox = new SuperToolbox();
            formFromSuperToolbox.Show();
        }

        private static void QuickBmsBatchToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var formFromQuickBmsBatch = new QuickBMSBatchExtractor.MainForm();
            formFromQuickBmsBatch.Show();
        }

        private static void Sofdec2ViewerToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "USM视频查看工具汉化版.exe");
                if (File.Exists(relativePath))
                {
                    Process.Start(relativePath);
                }
                else
                {
                    MessageBox.Show("未找到USM视频查看工具汉化版.exe，请检查路径。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动USM视频查看工具汉化版.exe出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void RadVideoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "radvideo64.exe");
                if (File.Exists(relativePath))
                {
                    Process.Start(relativePath);
                }
                else
                {
                    MessageBox.Show("未找到radvideo64.exe文件，请检查路径。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动radvideo64.exe时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void AddMenuItemsToPluginMenu(ToolStripMenuItem pluginMenu)
        {
            var extractorMenuItem = CreateDefaultExtractorMenuItem();
            var removerMenuItem = CreateDefaultRemoverMenuItem();
            var sofdec2ViewerMenuItem = CreateSofdec2ViewerMenuItem();
            var radVideoMenuItem = CreateRadVideoMenuItem();
            var quicbmsbatchMenuItem = CreateQuickBmsMenuItem();
            var superToolboxMenuItem = CreateSuperToolboxMenuItem();

            pluginMenu.DropDownItems.Add(extractorMenuItem);
            pluginMenu.DropDownItems.Add(removerMenuItem);
            pluginMenu.DropDownItems.Add(sofdec2ViewerMenuItem);
            pluginMenu.DropDownItems.Add(radVideoMenuItem);
            pluginMenu.DropDownItems.Add(quicbmsbatchMenuItem);
            pluginMenu.DropDownItems.Add(superToolboxMenuItem);
        }

        public static void AddMenuItemsToMainForm(MainForm mainForm)
        {
            var pluginToolStripMenuItem = mainForm.MenuStrip1.Items["toolStripMenuItem20"] as ToolStripMenuItem;
            if (pluginToolStripMenuItem != null)
            {
                AddMenuItemsToPluginMenu(pluginToolStripMenuItem);
            }
        }
    }
}
