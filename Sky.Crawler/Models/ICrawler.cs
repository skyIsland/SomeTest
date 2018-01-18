using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky.Crawler.Models
{
    public interface ICrawler<TModel,THtmlDoc>
    {
        /// <summary>
        /// 获取请求响应
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        string GetUrlString(string address, Encoding encoding = null);

        /// <summary>
        /// 获取请求响应并转为IHtmlDocument
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        THtmlDoc GetHtmlDocumet(string address, Encoding encoding = null);

        /// <summary>
        /// 开始
        /// </summary>
        void Start();

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        List<TModel> GetList(string address = "");

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="category">分类名称</param>
        /// <param name="cid">分类Id</param>
        /// <param name="index">页码</param>
        void Do_Task(string savePath, string category, int cid, int index);


    }
}
