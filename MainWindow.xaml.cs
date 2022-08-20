using System.Diagnostics;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Panuon.UI.Silver;

namespace Android_App_Install
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectPath(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog SelectAPKPath = new FolderBrowserDialog
            {
                Description = "请选择APK所在文件夹"
            };
            if (SelectAPKPath.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            { 
                APKPath.Text = SelectAPKPath.SelectedPath;
                LoadPathAPK(sender, e);
            }
        }
        public void LoadPathAPK(object sender, RoutedEventArgs e)
        {
            if (SelectList.Items.Count != 0) SelectList.Items.Clear();
            if (!string.IsNullOrEmpty(APKPath.Text)) if (Directory.Exists(APKPath.Text))
            {
                SelectList.ItemsSource = Directory.GetFiles(APKPath.Text, "*.apk");
                //for (int i = 0; i < Directory.GetFiles(APKPath.Text, "*.apk").Length; i++) SelectList.Items.Add(Directory.GetFiles(APKPath.Text, "*.apk").GetValue(i));
                if (SelectList.Items.Count == 0) MessageBoxX.Show("加载失败：\n没有足够的权限访问或该目录下没有APK文件");
            }
            else MessageBoxX.Show("请选择文件路径");
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (SelectList.SelectedItem != null) InstallList.Items.Add(SelectList.SelectedItem);
        }

        private void AddAll(object sender, RoutedEventArgs e)
        {
            if (InstallList.Items.Count != 0) InstallList.Items.Clear();
            for (int i = 0; i < SelectList.Items.Count; i++) InstallList.Items.Add(SelectList.Items.GetItemAt(i));
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (InstallList.SelectedItem != null) InstallList.Items.Remove(InstallList.SelectedItem);
        }

        private void RemoveAll(object sender, RoutedEventArgs e)
        {
            if (InstallList.Items.Count != 0) InstallList.Items.Clear();
        }

        private void Install(object sender, RoutedEventArgs e)
        {
            LogOutput.Install(InstallList.SelectedItem.ToString());
        }

        private void InstallAll(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < SelectList.Items.Count; i++)
            {
                LogOutput.Install(SelectList.Items.GetItemAt(i).ToString());
            }
        }
    }
}
