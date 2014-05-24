using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace VKScriptEval
{
    public partial class Form1 : Form
    {
        public Token Token;
        WebClient wc = new WebClient();
        private readonly string _TOKEN_FILE_PATH = new FileInfo(Application.ExecutablePath).DirectoryName + "\\token.bin";
        public Form1(string[] param)
        {
            if (File.Exists(_TOKEN_FILE_PATH))
            {
                if (File.ReadAllText(_TOKEN_FILE_PATH).Length > 0)
                {
                    Token = new Token();
                    string str = File.ReadAllText(_TOKEN_FILE_PATH);
                    var t = str.Split(' ');
                    Token.AccessToken = t[0];
                    Token.ExpiresIn = Convert.ToInt32(t[1]);
                    Token.UserId = Convert.ToInt32(t[2]);
                }
            }
            wc.Encoding = Encoding.UTF8;
            if (Token == null)
            {
                while (true)
                {
                    LoginForm lf = new LoginForm();
                    if (lf.ShowDialog() == DialogResult.OK)
                    {
                        Token = lf.Token;
                        File.WriteAllText(_TOKEN_FILE_PATH, lf.Token.AccessToken + " " + lf.Token.ExpiresIn + " " + lf.Token.UserId);
                        break;
                    }
                }
            }
            InitializeComponent();
            if (param.Length > 0)
            {
                tbCode.Text = File.ReadAllText(param[0]);
            }
            tbToken.Text = Token.AccessToken;
            
        }
        private string _methodStringJSON = "https://api.vk.com/method/execute";
        private string _methodStringXML = "https://api.vk.com/method/execute.xml";
        private void btnExecute_Click(object sender, EventArgs e)
        {
            string code = tbCode.Text.Replace("+", "%2B");
            Uri url;
            if(cbFormat.SelectedItem=="XML")
                url = new Uri(_methodStringXML + "?code=" + code + "+&access_token=" + Token.AccessToken);
            else
                url = new Uri(_methodStringJSON + "?code=" + code + "+&access_token=" + Token.AccessToken);

            tbQueryString.Text = url.ToString() ;
            tbResponse.Text = wc.DownloadString(url);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName + ".vks", tbCode.Text);        
            }
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            tbResponse.Text = wc.DownloadString(tbQueryString.Text);
        }
    }
}
