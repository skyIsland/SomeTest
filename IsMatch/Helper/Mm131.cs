using AngleSharp;
using IsMatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace IsMatch.Helper
{
    /// <summary>
    /// MM131 图片处理 http://www.mm131.com
    /// </summary>
    public class Mm131
    {
        /// <summary>
        /// 保存路径
        /// </summary>
        private readonly string FilesSaveSrc = "E:\\Study\\CrawlerResult\\MM131";
        /// <summary>
        /// 分类数据 
        /// </summary>
        private string[] Categorys = new string[] { "qingchun", "xiaohua", "chemo", "qipao", "mingxing", "xinggan" };
        /// <summary>
        /// 地址 ({0}和{1}对应)
        /// </summary>
        private string Address = "http://www.mm131.com/{0}/list_{1}_{2}.html";
        /// <summary>
        /// 已下载图片链接
        /// </summary>
        private List<string> ImageUrlList = new List<string>();

        /// <summary>
        /// 获取指定链接下的图片(默认随机获取某个分类下的第一页图片)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public List<Belle> GetListBelle(string url = "")
        {
            var flag = url.IsNullOrWhiteSpace();
            try
            {
                // 实例化Random
                var rand = new Random();

                //生成一个随机数
                var rnd = rand.Next(1, Categorys.Length);

                // 地址 todo:第一页不存在list_index_index
                var address = flag ? string.Format(Address, Categorys[rnd], rnd + 1, 2) : url;

                // 根据class获取html元素 todo:选择器错误 应该是每张图片
                var cells = GetHtmlString(address).QuerySelectorAll(".list-left dd");

                // We are only interested in the text - select it with LINQ
                List<Belle> list = new List<Belle>();

                // 
                var ii = 0;
                foreach (var item in cells)
                {
                    var title = item.QuerySelector("a").TextContent;
                    var detailUrl = item.QuerySelector("a").GetAttribute("href");

                    // 再次请求地址
                    var detailDoc = GetHtmlString(detailUrl);

                    // 图片页数 
                    var pageNum = detailDoc.QuerySelectorAll(".page-ch").First().TextContent.Replace("页", "").Replace("共", "").ToInt(-1);

                    // 强行20页
                    pageNum = pageNum > 20 ? 20 : pageNum;
                    if (pageNum > 0)
                    {
                        // 
                        var belle = new Belle();
                        belle.Title = title;

                        string nextUrl = string.Empty;
                        for (int i = 0; i < pageNum; i++)
                        {
                            //IHtmlDocument result = new HtmlDocument();
                            if (i == 0)
                            {
                                nextUrl = detailUrl;
                            }
                            else
                            {
                                if (!nextUrl.IsNullOrWhiteSpace() && nextUrl.Contains("_"))
                                {
                                    nextUrl = nextUrl.Split('_')[0] + "_" + (i + 1) + ".html";
                                }
                                else
                                {
                                    nextUrl = nextUrl.Replace(".html", "_" + (i + 1) + ".html");
                                }
                            }
                            var nextDoc = GetHtmlString(nextUrl);
                            var result = nextUrl
                                         + "-"
                                         + nextDoc
                                             .QuerySelector("body > div.content > div.content-pic > a > img")
                                             .GetAttribute("src");
                            belle.ImageUrls.Add(result);
                        }
                        list.Add(belle);
                    }
                    if(flag)
                        break;
                    // 遍历太多了 太卡了(对方服务器卡)  只取20条
                    ii++;
                    if (ii > 5)
                        break;
                }
                return list;

            }
            catch (Exception ex)
            {
                NewLife.Log.XTrace.LogPath = "D:\\Log";
                NewLife.Log.XTrace.WriteLine(" {0}获取图片链接失败 {1} 路径{2} 堆栈{3}", url, ex.Message, ex.Source, ex.StackTrace);
                return new List<Belle>()
                {
                    new Belle()
                    {
                        Title = "发生错误"+ex.Message,
                        ImageUrls =
                        {
                            "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=915737035,1612513878&fm=27&gp=0.jpg"
                        }
                    }
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="category">分类拼音</param>
        /// <param name="cid">分类id</param>
        /// <param name="index">页码</param>
        private void Do_Task(string savePath, string category, int cid,int index)
        {          

            // 构造链接
            var url = string.Empty;
            if (index == 0)
            {
                url = $"http://www.mm131.com/{category}";
            }
            else
            {
                url = string.Format(Address, category, cid, index);
            }

            // 获取图片列表
            var imageList = GetListBelle(url);

            // 日志
            NewLife.Log.XTrace.LogPath = "D:\\Log";
            NewLife.Log.XTrace.WriteLine("开始抓取cid ={0} 第 {1}页", category, index);

            //开始下载
            foreach (var imgs in imageList)
            {
                var imgList = imgs.ImageUrls;
                if (imgs.Title.Contains("错误"))
                    continue;
                var newSavePath = savePath + "\\" + imgs.Title.Replace(" ","");
                if (!Directory.Exists(newSavePath))
                {
                    Directory.CreateDirectory(newSavePath);
                }
                foreach (var imgInfo in imgList)
                {
                    var refer= imgInfo.Split("-")[0];
                    var img = imgInfo.Split("-")[1];
                    if (!ImageUrlList.Contains(img))
                    {
                        using (var wc = new WebClient())
                        {
                            try
                            {
                                var header = new WebHeaderCollection
                                {
                                    {HttpRequestHeader.Referer, refer},
                                    {
                                        HttpRequestHeader.UserAgent,
                                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36"
                                    }
                                };
                                wc.Headers = header;

                                wc.DownloadFile(img, Path.Combine(newSavePath, Path.GetFileName(img)));
                                NewLife.Log.XTrace.LogPath = "D:\\Log";
                                NewLife.Log.XTrace.WriteLine("{0} 下载成功", img);
                            }
                            catch (Exception ex)
                            {
                                NewLife.Log.XTrace.LogPath = "D:\\Log";
                                NewLife.Log.XTrace.WriteLine("{0} 下载失败 {1} 路径{2} 堆栈{3}", img, ex.Message, ex.Source, ex.StackTrace);
                            }
                        }
                        ImageUrlList.Add(img);
                    }
                }
               
            }
            // 递归调用
            //Do_Task(savePath, category, index + 1);
        }
        /// <summary>
        /// 暴力获取所有图片....
        /// </summary>
        public void DownloadAllImage()
        {
            foreach (var category in Categorys)
            {
                var lastIndexAddr = string.Empty;
                var categoryIndex = Categorys.ToList().IndexOf(category);
                if (categoryIndex == 0)
                {
                    lastIndexAddr = $"http://www.mm131.com/{category}";
                }
                else
                {
                    lastIndexAddr = string.Format(Address, category, Categorys.ToList().IndexOf(category) + 1, 1);
                }
                var lastPage = GetLastIndex(lastIndexAddr);
                //Task[] tasks = new Task[lastPage];
                //for (int i = 0; i < lastPage; i++)
                //{
                //    var index = i;
                //    tasks[i] = Task.Factory.StartNew(() => { Do_Task(FilesSaveSrc + "\\" + category, category, Categorys.ToList().IndexOf(category) + 1,index); });                  
                //}
                //Task.WaitAll(tasks);
                for (int i = 0; i < lastPage; i++)
                {
                     Do_Task(FilesSaveSrc + "\\" + category, category, Categorys.ToList().IndexOf(category) + 1, i);;
                }
            }

        }

        /// <summary>
        /// 获取分类的最后一页
        /// </summary>
        /// <returns></returns>
        public int GetLastIndex(string address)
        {
            var document = GetHtmlString(address);
            // list_1_31
            var lastPageStr = document
                            .QuerySelector("dd.page > a:last-of-type")
                            .GetAttribute("href");

            // 处理链接 得到末页
            var startIndex = lastPageStr.LastIndexOf("_", StringComparison.Ordinal);
            var endIndex = lastPageStr.LastIndexOf(".", StringComparison.Ordinal);
            var length = endIndex - startIndex;
            var lastPage = lastPageStr.Substring(startIndex+1, length-1);

            return lastPage.ToInt(-1);
        }

        /// <summary>
        /// 简单获取html源代码并且转换为IHtmlDocument
        /// </summary>
        /// <returns></returns>
        public IHtmlDocument GetHtmlString(string address,Encoding encoding=null)
        {
            // 注册模块
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (encoding == null)
            {
                encoding=Encoding.GetEncoding("GB2312");
            }
            var str = string.Empty;
            using (var wc = new WebClient())
            {
                wc.Encoding = encoding;
                str = wc.DownloadString(address);
            }
            var parser=new HtmlParser();
            return parser.Parse(str);
        }
    }
}
