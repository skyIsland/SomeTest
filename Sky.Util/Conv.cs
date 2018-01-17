using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky.Util
{
    public class Conv
    {
        /// <summary>
        /// 转换为整形
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static int ToInt(string data)
        {
            int.TryParse(data, out int result);
            return result;
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>

        public static List<Guid> ToGuidList(string guids)
        {
            var listGuid=new List<Guid>();
            if (string.IsNullOrWhiteSpace(guids))
            {
                return listGuid;
            }
            var arrayGuid = guids.Split(',');
            listGuid.AddRange
            (
                from each 
                in arrayGuid
                where !string.IsNullOrWhiteSpace(each)
                select new Guid(each)
            );
            return listGuid;
        }
    }
}
