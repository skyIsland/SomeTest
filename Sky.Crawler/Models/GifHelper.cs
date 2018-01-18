﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ivony.Html;
using Ivony.Html.Parser;
using Newtonsoft.Json;

namespace Sky.Crawler.Models
{
    /// <summary>
    /// http://tu.duowan.com/m/bxgif
    /// </summary>
    public class GifHelper //: ICrawler<ImageInfo<DuoWan.DwResult>, IHtmlDocument>
    {
        private string FileSavePath = ConfigurationManager.AppSettings["SaveFilePath"];

        private string Address = "http://tu.duowan.com/m/bxgif?offset={0}&order=created&math=0.594337572215683";

        private string DetailAddress = "http://tu.duowan.com/index.php?r=show/getByGallery/&gid={0}&_=1516254941826";

        public void Start()
        {
            Do_Task(FileSavePath, 1);
        }


        /// <summary>
        /// 网页源代码
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string GetUrlString(string address, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding("UTF8");
            }
            var str = string.Empty;
            using (var wc = new WebClient())
            {
                wc.Encoding = encoding;
                str = wc.DownloadString(address);
            }
            return str;
        }

        /// <summary>
        /// 获取网页源代码并转换为HtmlDocument
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public IHtmlDocument GetHtmlDocumet(string address, Encoding encoding = null)
        {
            var resultStr = GetUrlString(address, encoding);
            return new JumonyParser().Parse(resultStr);
        }


        public void Do_Task(string savePath,int index)
        {
            // 构造请求地址
            var address = string.Format(Address, index * 30);

            // 请求地址的图片信息
            var firstImgList = GetList(address);

            var ids = (List<string>)firstImgList["ids"];
            ids.ForEach(detail =>
            {
                // 每一个图片集
                if (!detail.IsNullOrWhiteSpace())
                {
                    var info = GetImageInfos(detail);
                    if (!info.Title.IsNullOrWhiteSpace())
                    {
                        var filePath = savePath + "\\" + info.Title.Trim();
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        var urls = info.Urls;
                        urls.ForEach(url =>
                        {
                            using (var wc = new WebClient())
                            {
                                wc.DownloadFile(url,Path.Combine(filePath,Path.GetFileName(url)??Guid.NewGuid().ToString().Substring(0,10)));
                            }
                        });                       
                    }
                }
            });
            // 递归调用
            var isMore = (bool) firstImgList["more"];

            if (isMore)
                Do_Task(savePath, index + 1);
        }

        /// <summary>
        /// 获取每一次请求的Ids
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Hashtable GetList(string address = "")
        {
            var listAddress=new List<string>();

            // 获取返回信息
            var result = JsonConvert.DeserializeObject<DuoWan.DwResult>(GetUrlString(Address));

            var document=new JumonyParser().Parse(result.html);

            var cells = document.Find("li>a");

            // 开始遍历
            foreach (var li in cells)
            {
                var detailUrl = li.Attribute("href").Value();

                // 获取id
                listAddress.Add((from each in detailUrl where each.ToString().ToInt(-1) > 0 select each).ToString());
            }

            //listAddress.ForEach(detail =>
            //{
            //    if (!detail.IsNullOrWhiteSpace())
            //    {
            //        var imgInfos = GetImageInfos(detail);
            //    }
            //});
            var o=new Hashtable()
            {
                {"more",result.more },
                {"ids", listAddress}
            };
            return o;
        }

        /// <summary>
        /// 获取每一个详情页的所有图片
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public ImageInfo GetImageInfos(string address)
        {
            var result = JsonConvert.DeserializeObject<DuoWan.DetailResult>(GetUrlString(address));
            if (result.gallery_title.IsNullOrWhiteSpace())
            {
                return  new ImageInfo();
            }

            var info=new ImageInfo();
            info.Title = result.gallery_title;

            // 开始遍历
            var data = result.picInfo;
            info.Urls = (from picinfo in data select picinfo.url).ToList();

            return info;
        }
                    
    }
}
