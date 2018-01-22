using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky.Crawler.Models
{
    public class ImageInfo
    {
        /// <summary>
        /// 图片名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片链接集合
        /// </summary>
        public List<string> Urls { get; set; }=new List<string>();

        ///// <summary>
        ///// 额外信息
        ///// </summary>
        //public TModel Data { get; set; }
    }
}
