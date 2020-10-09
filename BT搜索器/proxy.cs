using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using BT搜索器.Properties;

namespace BT搜索器
{
    public partial class proxy : Form
    {
        public static string proxy_ip;
        public static string proxy_port;
        public static string user_agent= "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1 QQBrowser/6.9.11079.201";
        public proxy()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, EventArgs e)
        {
            string proxy = textBox_Proxy.Text.Trim();
            if (string.IsNullOrEmpty(proxy))
            {
                proxy_ip = proxy_port = "";
                
                this.Close();
                return;
                
            }

            // 正则表达式提取
            Regex re = new Regex(@":(\d+)$"); //提取端口号
            string proxy_port_ = re.Match(proxy).Groups[1].Value;

            re = new Regex(@"^\d+.\d+.\d+.\d+");//提取ip
            string proxy_ip_ = re.Match(proxy).Groups[0].Value;
            if (String.IsNullOrEmpty(proxy_port_) || String.IsNullOrEmpty(proxy_ip_))
            {
                MessageBox.Show("请输入正确的代理：\n格式：[ip:port]", "提示", MessageBoxButtons.OK);
            }
            else {
                proxy_port = proxy_port_;
                proxy_ip = proxy_ip_;
            }
            user_agent = textBox_UA.Text.Trim();
            if (setting.WtrieSetting("代理", "UA", user_agent) && setting.WtrieSetting("代理", "ip", proxy_ip) && setting.WtrieSetting("代理", "port", proxy_port))
            {

            }
            else {
                MessageBox.Show("设置文件写入失败","提示",MessageBoxButtons.OK);
            }
            
            this.Close();
        }

        private void proxy_Load(object sender, EventArgs e)
        {
            textBox_UA.Text = user_agent;
            if (!string.IsNullOrEmpty(proxy.proxy_ip) && !string.IsNullOrEmpty(proxy.proxy_port))
            {
                textBox_Proxy.Text = string.Format("{0}:{1}", proxy_ip, proxy_port);
            }
            
        }
    }
}
