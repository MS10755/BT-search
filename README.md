# BT-search

**界面预览：**

![UI界面](https://raw.githubusercontent.com/MS10755/BT-search/master/pics/%E6%8D%95%E8%8E%B7.PNG)

C#编写的BT爬虫项目，也是本人第一个C#项目。

### 项目文件说明
---

- **平台：VS2019/.NET Framework 4.7.2**

| 文件           | 说明                                                         |
| -------------- | ------------------------------------------------------------ |
| ExcelOutPut.cs | Excel文件输出，使用NPOI库                                    |
| HtmlRequest.cs | 发送http请求                                                 |
| Spider.cs      | 爬虫，用于爬取磁力信息，使用HtmlAgilityPack库提供的Xpath解析数据 |
| setting.cs     | 配置文件读写                                                 |

### 其它说明
---

**本项目只能用于学习交流，不提供编译好的程序。**

### 更新说明
---
**2020/11/12 ** 种子帝和BTSOW的搜索地址已更改，需要修改Spider.cs文件中两个爬虫类的this.api_url属性，不然什么都搜不到。因为这类网站地址经常变动，因此我就不经常更新了（其实是懒）。具体地址可以在www.jubt.cf找到

### 关于作者
---

- **MS10755**
- **博客：** http://juanpi.zicp.net
