using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VKScriptEval
{
    public partial class LoginForm : Form
    {
        private const string ALL_ACCESS="notify,friends,photos,audio,video,docs,notes,pages,status,offers,questions,wall,groups,messages,notifications,stats,ads,offline";
        public LoginForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            Clipboard.SetText("https://oauth.vk.com/authorize?client_id=3394784&scope=" + ALL_ACCESS + "&display=page&response_type=token");
            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=3394784&scope=" + ALL_ACCESS + "&display=page&response_type=token");
            webBrowser1.Navigated += webBrowser1_Navigated;
        }
        public Token Token;
        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string url = e.Url.ToString();
            if (url.IndexOf("access_token") != -1)
            {
                int pos1 = url.IndexOf("access_token=") + "access_token=".Length;
                int pos2 = url.IndexOf('&', pos1);
                string token = url.Substring(pos1, pos2 - pos1);
                pos1 = url.IndexOf("expires_in=") + "expires_in=".Length;
                pos2 = url.IndexOf("&", pos1);
                int expires_in = Convert.ToInt32(url.Substring(pos1, pos2 - pos1));
                int user_id = Convert.ToInt32(url.Substring(url.IndexOf("user_id=") + "user_id=".Length));
                Token = new VKScriptEval.Token();
                Token.AccessToken = token;
                Token.ExpiresIn=expires_in;
                Token.UserId = user_id;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
