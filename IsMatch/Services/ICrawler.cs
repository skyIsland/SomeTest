using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsMatch.Services
{
    public interface ICrawler<TModel>
    {
        /// <summary>
        /// 开始
        /// </summary>
        void Start();

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <returns></returns>
        List<TModel> GetList();

        /// <summary>
        /// 下载图片
        /// </summary>
        void Do_Task();
    }
}
