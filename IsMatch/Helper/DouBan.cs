﻿using AngleSharp;
using IsMatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IsMatch.Helper
{
    /// <summary>
    /// 豆瓣图片处理 url:https://www.dbmeinv.com/dbgroup/show.htm
    /// </summary>
    public class DouBan
    {
        /// <summary>
        /// 保存路径
        /// </summary>
        private readonly string FilesSaveSrc = "E:\\Study\\CrawlerResult\\DouBan";
        /// <summary>
        /// 分类ID数据 2-胸 3-腿 4-脸 5-杂 6-臀 7-袜子
        /// </summary>
        private int[] Ids = new int[] { 2, 3, 4, 5, 6, 7 };
        /// <summary>
        /// 豆瓣地址
        /// </summary>
        private string DouBanUrl = "https://www.dbmeinv.com/dbgroup/show.htm?cid={0}&pager_offset={1}";
        /// <summary>
        /// 已下载图片链接
        /// </summary>
        private List<string> ImageUrlList = new List<string>();
        //public DouBan()
        //{
        //    FilesSaveSrc=   
        //}
        /// <summary>
        /// 获取指定链接下的图片(默认随机获取某个分类下的第一页图片)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public List<Belle> GetListBelle(string url="")
        {
            // 随机数
            var rand = new Random();

            // 设置配置以支持文档加载
            var config = Configuration.Default.WithDefaultLoader();

            // 豆瓣地址
            var address = string.IsNullOrWhiteSpace(url)?"https://www.dbmeinv.com/dbgroup/show.htm?cid=" + Ids[rand.Next(Ids.Length)]:url;

            // 请求豆辨网
            var document = BrowsingContext.New(config).OpenAsync(address);

            // 根据class获取html元素
            var cells = document.Result.QuerySelectorAll(".panel-body li");

            // We are only interested in the text - select it with LINQ
            List<Belle> list = new List<Belle>();
            foreach (var item in cells)
            {
                var belle = new Belle
                {
                    Title = item.QuerySelector("img").GetAttribute("title"),
                    ImageUrl = item.QuerySelector("img").GetAttribute("src")
                };
                list.Add(belle);
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="cid">分类ID</param>
        /// <param name="index">页码</param>
        private void Do_Task(string savePath, int cid, int index)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // 构造链接
            var url = string.Format(DouBanUrl, cid, index);

            // 获取图片列表
            var imageList = GetListBelle(url);

            // 日志
            NewLife.Log.XTrace.LogPath = "D:\\Log";
            NewLife.Log.XTrace.WriteLine("开始抓取cid ={0} 第 {1}页", cid, index);

            //开始下载
            foreach (var img in imageList)
            {
                var imgUrl = img.ImageUrl;
                if (!ImageUrlList.Contains(imgUrl))
                {
                    var dirImageCount = Directory.GetDirectories(savePath);
                    if (dirImageCount.Length == 0)//子文件夹个数为0 创建第一个文件夹
                    {
                        Directory.CreateDirectory(savePath + "\\1");
                        dirImageCount = Directory.GetDirectories(savePath);
                    }
                    //最后一个文件夹的图片数量
                    var imageCount = Directory.GetFiles(savePath + "\\" + dirImageCount.Length);
                    if (imageCount.Length >= 500)//大于等于500时新建文件夹
                    {
                        Directory.CreateDirectory(savePath + "\\" + (dirImageCount.Length + 1));
                    }
                    //重新获取一下子文件夹数量
                    dirImageCount = Directory.GetDirectories(savePath);

                    using (var wc = new WebClient())
                    {
                        try
                        {
                            wc.DownloadFile(imgUrl, Path.Combine(savePath + "\\" + dirImageCount.Length, Path.GetFileName(imgUrl)));
                            NewLife.Log.XTrace.LogPath = "D:\\Log";
                            NewLife.Log.XTrace.WriteLine("{0} 下载成功", imgUrl);
                        }
                        catch (Exception ex)
                        {
                            NewLife.Log.XTrace.LogPath = "D:\\Log";
                            NewLife.Log.XTrace.WriteLine("{0} 下载失败 {1} 路径{2} 堆栈{3}", imgUrl,ex.Message,ex.Source,ex.StackTrace);
                        }
                    }
                    ImageUrlList.Add(img.ImageUrl);
                }
            }
            // 递归调用
            Do_Task(savePath, cid, index + 1);
        }
        /// <summary>
        /// 暴力获取所有图片....
        /// </summary>
        public void DownloadAllImage()
        {
            foreach (var cid in Ids)
            {
                // 还是开一个任务吧
                Task.Factory.StartNew(() =>
                {
                    Do_Task(FilesSaveSrc+"\\"+cid, cid, 1);
                });
            }
        }
    }
}
