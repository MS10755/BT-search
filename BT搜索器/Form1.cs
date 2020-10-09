using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Org.BouncyCastle.Asn1.Cms;

namespace BT搜索器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void search_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(KeyWord.Text))
            {
                return;
            }
            listView1.Items.Clear();
            StatusCode.Text = "正在搜索，请稍后！";
            //开启新线程后台请求网页，不影响前台UI
            new Thread(() =>
            {
                //使用委托来刷新UI
                this.Invoke((Action)delegate {
                    zhongzidiStatus.Text = "种子帝：" + Thread.CurrentThread.ThreadState.ToString();
                });
                zhongzidi spider1 = new zhongzidi(KeyWord.Text.Trim());
                ArrayList results = spider1.GetData();
                foreach (Dictionary<string, string> result in results)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = spider1.source;
                    item.SubItems.Add(result["title"]);
                    item.SubItems.Add(result["date"]);
                    item.SubItems.Add(result["size"]);
                    item.SubItems.Add(result["hot"]);
                    item.SubItems.Add(result["link"]);

                    //使用委托来刷新UI
                    this.Invoke((Action)delegate {
                        listView1.BeginUpdate();
                        listView1.Items.Add(item);
                        listView1.EndUpdate();
                    });
                }
                this.Invoke((Action)delegate {
                    zhongzidiStatus.Text = "种子帝：线程已结束";
                    StatusCode.Text = string.Format("搜索到：{0}项", listView1.Items.Count);
                });
            }).Start();
            
            //开启新线程后台请求网页，不影响前台UI
            new Thread(() =>
            {
                //使用委托来刷新UI
                this.Invoke((Action)delegate {
                    btsowStatus.Text = "BTSOW：" + Thread.CurrentThread.ThreadState.ToString();
                });
                BTSOW spider1 = new BTSOW(KeyWord.Text.Trim());
                ArrayList results = spider1.GetData();
                foreach (Dictionary<string, string> result in results)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = spider1.source;
                    item.SubItems.Add(result["title"]);
                    item.SubItems.Add(result["date"]);
                    item.SubItems.Add(result["size"]);
                    item.SubItems.Add(result["hot"]);
                    item.SubItems.Add(result["link"]);

                    //使用委托来刷新UI
                    this.Invoke((Action)delegate {
                        listView1.BeginUpdate();
                        listView1.Items.Add(item);
                        listView1.EndUpdate();
                    });
                }
                this.Invoke((Action)delegate {
                    btsowStatus.Text = "BTSOW：线程已结束";
                    StatusCode.Text = string.Format("搜索到：{0}项", listView1.Items.Count);
                });
            }).Start();
            
            

        }
        private class ListViewItemComparer : IComparer {
            private int col;
            public int Compare(object x, object y)
            {
                int returnVal = -1;
                 returnVal = String.Compare(((ListViewItem) x).SubItems[col].Text,((ListViewItem) y).SubItems[col].Text);
                 return returnVal;
            }
    }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer();
            listView1.Sort();
        }

        private void 复制链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count==0)
            {
                return;
            }
            string link = listView1.SelectedItems[0].SubItems[5].Text;
            try
            {
                Clipboard.SetText(link);
                if (Clipboard.GetText() == link)
                {
                    MessageBox.Show("链接复制成功", "信息", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("链接复制失败", "信息", MessageBoxButtons.OK);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("剪切板写入失败", "信息", MessageBoxButtons.OK);
            }
           

        }

        private void KeyWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                search_Click(sender,EventArgs.Empty);
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            proxy form = new proxy();
            form.ShowDialog();
        }

        private void 查看详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            detail form = new detail();
            form.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //加载配置文件
            if (File.Exists(Application.StartupPath+"/settings.ini"))
            {
                proxy.proxy_ip = setting.ReadSetting("代理", "ip");
                proxy.proxy_port = setting.ReadSetting("代理", "port");
                proxy.user_agent = setting.ReadSetting("代理", "UA");
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择想要保存的连接！\n按住：\nCTRL键+鼠标左键   单选\nCTRL+A键  全选","提示");
            }
            else {
                //将选中的项目写入_Xpath结构体
                Spider._xpath[] data= new Spider._xpath[listView1.SelectedItems.Count];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i].source = listView1.SelectedItems[i].SubItems[0].Text;
                    data[i].title = listView1.SelectedItems[i].SubItems[1].Text;
                    data[i].date = listView1.SelectedItems[i].SubItems[2].Text;
                    data[i].size = listView1.SelectedItems[i].SubItems[3].Text;
                    data[i].hot = listView1.SelectedItems[i].SubItems[4].Text;
                    data[i].link = listView1.SelectedItems[i].SubItems[5].Text;
                }
                string filename = string.Format("{0}\\{1}.xls", Application.StartupPath, DateTime.Now.ToFileTime());
               bool res= ExcelOutPut.SaveExcel(filename,data);
                if (res)
                {
                    MessageBox.Show("文件:\n" + filename + "\n保存成功", "提示",MessageBoxButtons.OK);
                }
                else {
                    MessageBox.Show("文件:\n" + filename + "\n保存失败", "提示",MessageBoxButtons.OK);
                }
            }
            listView1.SelectedItems.Clear();
        }
    }
}
