using Panuon.UI.Silver;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Android_App_Install
{
    /// <summary>
    /// LogOutput.xaml 的交互逻辑
    /// </summary>

    public partial class LogOutput : WindowX
    {
        public static LogOutput i;
        public LogOutput()
        {
            i = this;
            InitializeComponent();
            Log.Text = "等待日志输出...\n";
        }

        public static string getLog = "";

        public static async void Install(string path)
        {
            await Task.Run(() =>
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = @" install """ + path + @"""";
                // 禁用操作系统外壳
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                i.Log.AppendText(process.StandardOutput.ReadToEnd());
                i.Log.AppendText(process.StandardError.ReadToEnd());
                process.WaitForExit();
                process.Close();
            });
        }

        public static string FastInstall(string path)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = @" install """ + path + @"""";
                // 禁用操作系统外壳
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                //i.Log.AppendText(process.StandardOutput.ReadToEnd());
                //i.Log.AppendText(process.StandardError.ReadToEnd());
                getLog += process.StandardOutput.ReadToEnd();
                getLog += process.StandardError.ReadToEnd();
                process.WaitForExit();
                process.Close();
                if (MainWindow.h.Progress.Value < MainWindow.h.Progress.Maximum - 10) MainWindow.h.Progress.Value += 10;
            }
            catch (Exception)
            {
                return "ERROR";
            }
            return getLog;
        }
    }
}
