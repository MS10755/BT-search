using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace BT搜索器
{
    class HtmlRequest
    {
        public static Dictionary<string,object> GetHtmlText(string url) {
            Dictionary < string, object> result = new Dictionary<string, object>();
            
            //创建请求
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);

            //如果设置了代理，则启动代理
            if (!string.IsNullOrEmpty(proxy.proxy_ip) && !string.IsNullOrEmpty(proxy.proxy_port))
            {
                rq.UserAgent = proxy.user_agent;
                rq.Proxy = new WebProxy(proxy.proxy_ip, Convert.ToInt32(proxy.proxy_port));
                //rq.Proxy = new WebProxy("150.138.253.72", 808);

            }
            else {
                rq.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1 QQBrowser/6.9.11079.201";
            }
            rq.AllowAutoRedirect = false;
            rq.Timeout = 10000;
            HttpWebResponse rp;
            try
            {
                rp = (HttpWebResponse)rq.GetResponse();
                Stream stream = rp.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                result.Add("code", rp.StatusCode);
                result.Add("body", reader.ReadToEnd());
                return result;
            }
            catch (Exception e)
            {
              
                return null;
            }
            
        }
    }
}
