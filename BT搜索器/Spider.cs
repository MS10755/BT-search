using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BT搜索器
{
    abstract class Spider {
        public  struct _xpath
        {
            public string source;
            public string title;
            public string date;
            public string size;
            public string hot;
            public string link;
        }
        //public _xpath _Xpath=new _xpath();
        public string KeyWord; //搜索的关键词
        public int max_page_num;//最大爬取的页数
        public string api_url { get; set; }
        public string source { get; set; }
        public abstract ArrayList GetData();
    }
    class zhongzidi: Spider
    {
        public zhongzidi(String key_word) {
            
            this.KeyWord = key_word;
            this.source = "种子帝";
            this.max_page_num = 5;
            this.api_url = "https://zhongziso6.com/list";
        }

        public override ArrayList GetData() {
            ArrayList results = new ArrayList();
            for (int pageindex = 1; pageindex <= this.max_page_num; pageindex++)
            {
                Dictionary<string, object> response = HtmlRequest.GetHtmlText(string.Format("{0}/{1}/{2}",api_url,KeyWord, pageindex));
                if (response is null)
                {
                    break;
                }
                //xpath 解析
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response["body"].ToString());
                HtmlNodeCollection titles = doc.DocumentNode.SelectNodes("//table//div[@class='text-left']//a");
                HtmlNodeCollection dates = doc.DocumentNode.SelectNodes("//table//tr[2]/td[1]/strong");
                HtmlNodeCollection sizes = doc.DocumentNode.SelectNodes("//table//tr[2]/td[2]/strong");
                HtmlNodeCollection hots = doc.DocumentNode.SelectNodes("//table//tr[2]/td[3]/strong");
                HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//table//tr[2]/td[4]/a");
                if (titles is null)
                {
                    break;
                }
                for (int i = 0; i < titles.Count; i++)
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    result.Add("title", titles[i].InnerText);
                    result.Add("date", dates[i].InnerText);
                    result.Add("size", sizes[i].InnerText);
                    result.Add("hot", hots[i].InnerText);
                    result.Add("link", links[i].Attributes["href"].Value);

                    results.Add(result);
                }
            }
            return results;
        }
    }
    class BTSOW : Spider {
        public BTSOW(String key_word)
        {
            this.api_url = "https://btsow.online/search";
            this.KeyWord = key_word;
            this.max_page_num = 1;
            this.source = "BTSOW";
        }

        public override ArrayList GetData()
        {
            ArrayList results = new ArrayList();
            for (int pageindex =1; pageindex <= max_page_num; pageindex++)
            {
                Dictionary<string,object> response= HtmlRequest.GetHtmlText(string.Format("{0}/{1}/page/{2}",api_url,KeyWord,pageindex));
                if (response is null)
                {
                    break;
                }
                //xpath 解析
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response["body"].ToString());
                HtmlNodeCollection titles = doc.DocumentNode.SelectNodes("//div[@class='row']/a");//属性title
                HtmlNodeCollection dates = doc.DocumentNode.SelectNodes("//div[@class='row']/div[2]");//text()
                HtmlNodeCollection sizes = doc.DocumentNode.SelectNodes("//div[@class='row']/div[1]");//text()
                HtmlNodeCollection links_ = doc.DocumentNode.SelectNodes("//div[@class='row']/a");//属性href
                if (titles is null)
                {
                    break;
                }
                for (int i = 0; i < titles.Count; i++)
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    result.Add("title", titles[i].Attributes["title"].Value);
                    result.Add("date", dates[i].InnerText);
                    result.Add("size", sizes[i].InnerText);
                    result.Add("hot", "--");
                    Regex reg = new Regex(@"hash/([\w|\d]+)");
                    string link=reg.Match(links_[i].Attributes["href"].Value).Groups[1].Value;
                    result.Add("link", string.Format("magnet:?xt=urn:btih:{0}",link));

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
