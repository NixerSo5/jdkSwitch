using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jdkSwitch
{
    public partial class Form1 : Form
    {


        string jdkpath = null;
        string basepath = null; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                comboBox1.Items.Add(subDirectory.Substring(basepath.Length+14, subDirectory.Length-14-basepath.Length));

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          string jhome =   System.Environment.GetEnvironmentVariable("JAVA_HOME",EnvironmentVariableTarget.Machine);
            if (jhome == "")
            { //C:\Program Files\Java\jdk1.8.0_351
              //开始安装jdk

            }
            else {
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
                System.Environment.SetEnvironmentVariable("JAVA_HOME",jdkpath, EnvironmentVariableTarget.Machine);
                MessageBox.Show("设置成功");

                // MessageBox.Show(localpath);
            }
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            jdkpath = basepath+ "/jdkSwitchJdks/" + comboBox1.SelectedItem.ToString();
         
        }
    }
}
