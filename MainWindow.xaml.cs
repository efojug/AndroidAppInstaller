using System.IO;
using System.Windows;
using System.Windows.Forms;
using Panuon.UI.Silver;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

namespace Android_App_Install
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public static MainWindow h;
        public MainWindow()
        {
            h = this;
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
            if (SelectList.Items.Count != 0) SelectList.Items.Refresh();
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
            bool put = true;
            if (SelectList.SelectedItem != null)
            {
                for (int i = 0; i < InstallList.Items.Count; i++)
                {
                    if (SelectList.SelectedItem == InstallList.Items.GetItemAt(i)) put = false;
                }
                if (put) InstallList.Items.Add(SelectList.SelectedItem);
            }
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
            if (InstallList.SelectedItem != null)
            {
                Stats.Text = "创建窗口";
                Progress.Value = 0;
                new LogOutput().Show();
                LogOutput.i.Log.Text = "";
                Progress.Value = 100;
                Stats.Text = "开始安装";
                Progress.Value = 0;
                LogOutput.Install(InstallList.SelectedItem.ToString());
                Stats.Text = "安装完成";
                Progress.Value = 100;
            }
        }

        public bool MultiThreadMode = false;

        public string IAO = "";

        private void InstallAll(object sender, RoutedEventArgs e)
        {
            if (InstallList.Items.Count != 0)
            {
                if (MultiThreadMode)
                {
                    Stats.Text = "创建窗口";
                    new LogOutput().Show();
                    LogOutput.i.Log.Text = "";
                    Progress.Value = 100;
                    Progress.Value = 0;
                    Stats.Text = "创建线程";
                    for (int i = 0; i < InstallList.Items.Count; i++)
                    {
                        int sbBug = i;
                        Thread thread = new Thread(() =>
                        {
                            IAO = LogOutput.FastInstall(InstallList.Items.GetItemAt(sbBug).ToString());
                        });
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                        if (Progress.Value < Progress.Maximum - 10) Progress.Value += 10;
                    }
                    if (IAO == "ERROR") MessageBoxX.Show("程序出现错误");
                    else LogOutput.i.Log.AppendText(IAO);
                }
                else
                {
                    Stats.Text = "创建窗口";
                    new LogOutput().Show();
                    Stats.Text = "开始安装";
                    for (int i = 0; i < InstallList.Items.Count; i++)
                    {
                        LogOutput.Install(InstallList.Items.GetItemAt(i).ToString());
                    }
                    Stats.Text = "安装完成";
                }
            }
        }

        private void EnableMultiThread(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Show("开启多线程模式会使安装速度变快，但可能出现无法预料的错误", "提示");
            MultiThreadMode = true;
        }

        private void DisableMultiThread(object sender, RoutedEventArgs e)
        {
            MultiThreadMode = false;
        }
    }
}
