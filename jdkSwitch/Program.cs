
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jdkSwitch
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {




            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            //判断当前登录用户是否为管理员
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                //如果是管理员，则直接运行Application.Run(new FrmMain());
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {


                MessageBox.Show("请使用管理员打开");
                Application.Exit();


            }
        }
    }
}