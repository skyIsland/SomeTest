using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;

namespace IsMatch.Util
{
    public static class AngelSharpDom
    {
        public static List<Belle> RenderDouBanHtml()
        {
            // 设置配置以支持文档加载
            var config = Configuration.Default.WithDefaultLoader();
            // 豆瓣地址
            var address = "https://www.dbmeinv.com/dbgroup/show.htm?cid=2";
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
    }   

    /// <summary>
    /// 解析html
    /// </summary>
    public class Belle
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
