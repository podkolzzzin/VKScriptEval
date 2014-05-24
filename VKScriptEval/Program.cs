using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VKScriptEval
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!PacketViewer.FileAssociation.IsAssociated)
            {
                PacketViewer.FileAssociation.Associate("VK script", "icon_64x64.png");
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
        }
    }
}
