using Sky.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Sky.Crawler.FlySign
{

    /// <summary>
    /// Fly社区签到
    /// </summary>
    public class FlySignIn
    {
        /// <summary>
        /// 签到地址
        /// </summary>
        private string Url = "http://fly.layui.com/sign/in";

        /// <summary>
        /// token todo:获取token (sign/status 该链接可得到当前签到状态以及token信息(值,过期时间)) 
        /// </summary>
        private string FormData { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        private string CookieData { get; set; }
        
        /// <summary>
        /// 构造函数传参
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="cookieData"></param>
        public FlySignIn(string cookieData,string formData= "1")
        {
            this.FormData = "token=" + formData;
            this.CookieData = cookieData;
        }

        /// <summary>
        /// 程序入口
        /// </summary>
        public void Start()
        {
            SignIn(Url, FormData, CookieData);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="cookieData"></param>
        private void SignIn(string url, string formData,string cookieData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            // 定义相关请求头
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add(HttpRequestHeader.Cookie, CookieData);
            request.Referer = "http://fly.layui.com/";
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";

            // post数据相关
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteData = Encoding.UTF8.GetBytes(formData);
            request.ContentLength = byteData.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteData, 0, byteData.Length);
            }

            // 发出请求
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    var resultStr = reader.ReadToEnd();

                    var resultObj = NewLife.Serialization.JsonHelper.ToJsonEntity<Result>(resultStr);

                    // todo:返回token过期时重新获取token
                    var msg = string.Format("Fly社区签到信息如下:<br>签到 {0},消息: {1}", resultObj.status == 0 ? "成功" : "失败",
                        resultObj.msg);
                    NewLife.Log.XTrace.Log.Info(msg);
                   // EmailHandler.SendSmtpEMail("ismatch@qq.com","Fly社区签到",msg,new EmailHandler.SendAcccount{SmtpHost = "SMTP.163.com"});
                }
            }

            //using (var wc = new WebClient())
            //{
            //    var header = new WebHeaderCollection
            //    {
            //        {HttpRequestHeader.Referer, "http://fly.layui.com/"},
            //        {
            //            HttpRequestHeader.UserAgent,
            //            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36"
            //        },
            //        {
            //            HttpRequestHeader.Cookie,"Hm_lvt_a353f6c055ee7ac4ac9d1af83b556ef4=1488963535,1489112224,1489546169; UM_distinctid=15e59c095a1218-0684c1f914c599-3e64430f-1fa400-15e59c095a24bf; CNZZDATA30088308=cnzz_eid%3D792246781-1502875390-http%253A%252F%252Fwww.layui.com%252F%26ntime%3D1516760465; fly-layui=s%3AzyYSaO8b3RspMIV9E1lzKW3J67Ef2uOd.eMRtFxRsPJwfwZrZd17bv4B2dGwzuWE2lS3mC8EazHA"
            //        },
            //        {
            //            HttpRequestHeader.ContentLength,buffer.Length.ToString()
            //        },
            //        //{
            //        //    HttpRequestHeader.ContentRange
            //        //}
            //    };
            //    wc.Headers = header;
            //    wc.
            //}

        }

        /// <summary>
        /// 获取token
        /// </summary>
        private void GetToken()
        {
            
        }
        private class Result
        {
            /// <summary>
            /// 返回状态 0 成功 1 失败
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 返回信息
            /// </summary>
            public string msg { get; set; }

            public Data data { get; set; }

            public class Data
            {
                /// <summary>
                /// 连续签到天数
                /// </summary>
                public int days { get; set; }

                /// <summary>
                /// 飞吻
                /// </summary>
                public int experience { get; set; }
                /// <summary>
                /// 签到状态
                /// </summary>
                public bool signed  { get; set; }
            }
        }
    }

}
