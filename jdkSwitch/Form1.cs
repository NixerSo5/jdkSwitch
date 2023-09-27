using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using System.Windows;


namespace jdkSwitch
{
    public partial class Form1 : Form
    {
        string zipfilepath = null;
        string zipfilename = null;
        string jdkpath = null;
        string basepath = null;

        private WebClient client;


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            creatSelectBox1();

            String[] downloadf = new string[] { "21", "17", "11", "8" };
            foreach (string jdk in downloadf)
            {
                comboBox2.Items.Add(jdk);
            }

        }

        private void creatSelectBox1()
        {
            comboBox1.Items.Clear();
            string localpath = System.Windows.Forms.Application.StartupPath;
            string dpath = localpath + "/jdkSwitchJdks";
            if (!Directory.Exists(localpath + "/jdkSwitchJdks"))
            {
                Directory.CreateDirectory(dpath);
            }
            string[] subDirectories = Directory.GetDirectories(dpath);
            basepath = localpath;
            foreach (string subDirectory in subDirectories)
            {
                Console.WriteLine(subDirectory.Length);
                Console.WriteLine(basepath.Length);
                comboBox1.Items.Add(subDirectory.Substring(basepath.Length + 14, subDirectory.Length - 14 - basepath.Length));

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {

                string jhome = System.Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine);
                if (jhome == "")
                { //C:\Program Files\Java\jdk1.8.0_351
                  //开始安装jdk

                }
                else
                {
                    string pathStr = System.Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
                    string[] pathArry = pathStr.Split(';');
                    ArrayList pathlist = new ArrayList(pathArry);
                    if (!pathlist.Contains("%JAVA_HOME%\\bin"))
                    {
                        pathlist.Add("%JAVA_HOME%\\bin");
                    }
                    pathArry = (string[])pathlist.ToArray(typeof(string));
                    pathStr = string.Join(";", pathArry);
                    System.Environment.SetEnvironmentVariable("Path", pathStr, EnvironmentVariableTarget.Machine);

                    // 查看是否选中  如果选中   更新变量  如果没有  提示选中或者下载
                    System.Environment.SetEnvironmentVariable("JAVA_HOME", jdkpath, EnvironmentVariableTarget.Machine);
                    MessageBox.Show("设置成功");

                    // MessageBox.Show(localpath);
                }
            }
            else
            {
                MessageBox.Show("没选择jdk");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            jdkpath = basepath + "/jdkSwitchJdks" + comboBox1.SelectedItem.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void WebClientDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show($"文件下载失败：{e.Error.Message}");
            }
            else
            {
                MessageBox.Show("文件下载成功！");
                UnZipJdk(zipfilepath, zipfilename);

            }

            client.Dispose();
        }

        private void UnZipJdk(string zipPath, string extractPath)
        {
            string localpath = System.Windows.Forms.Application.StartupPath;
            string dpath = localpath + "/jdkSwitchJdks";
            extractPath = dpath;

            using (ZipFile zipFile = ZipFile.Read(zipPath))
            {
                zipFile.ExtractProgress += ZipFile_ExtractProgress;

                foreach (ZipEntry entry in zipFile)
                {
                    if (entry.IsDirectory)
                    {
                        string entryFileName = entry.FileName;
                        string fullZipToPath = Path.Combine(extractPath, entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);

                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }





                    }
                    else { entry.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently); }





                }
            }


            MessageBox.Show("Extraction complete!");
            creatSelectBox1();
        }







        private void ZipFile_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                int progress = (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer);
                progressBar2.Value = progress;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                String[] downloadlist = new string[] { "https://download.java.net/java/GA/jdk21/fd2272bbf8e04c3dbaee13770090416c/35/GPL/openjdk-21_windows-x64_bin.zip"
                , "https://download.java.net/openjdk/jdk17/ri/openjdk-17+35_windows-x64_bin.zip",
                "https://download.java.net/openjdk/jdk11.0.0.1/ri/openjdk-11.0.0.1_windows-x64_bin.zip",
                "https://download.java.net/openjdk/jdk8u43/ri/openjdk-8u43-windows-i586.zip"};
                //chat to int
                int index = comboBox2.SelectedIndex;
                String[] namelist = new string[] { "jdk21.zip", "jdk17.zip", "jdk11.zip", "jdk8.zip" };

                string fileUrl = downloadlist[index];
                string localpath = System.Windows.Forms.Application.StartupPath;
                string savePath = localpath + "\\jdkSwitchJdks\\" + namelist[index];
                zipfilepath = savePath;
                client = new WebClient();
                client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                client.DownloadFileCompleted += WebClientDownloadCompleted;


                try
                {
                    client.DownloadFileAsync(new Uri(fileUrl), @savePath);
                //  UnZipJdk(zipfilepath, zipfilename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"文件下载失败：{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("没有选择要下载的版本!");
            }

        }

    }
}
