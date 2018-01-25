using MyTest.Config;
using Sky.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using NewLife.Serialization;

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
        private string SignUrl = "http://fly.layui.com/sign/in";

        private string StatusUrl = "http://fly.layui.com/sign/status";
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
        public FlySignIn(string cookieData, string formData = "1")
        {
            this.FormData = "token=" + formData;
            this.CookieData = cookieData;
        }

        /// <summary>
        /// 程序入口
        /// </summary>
        public void Start()
        {
            var signed = GetStatus(StatusUrl, CookieData);
            if (!signed)
            {
                SignIn(SignUrl, FormData, CookieData);
            }
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData">token参数</param>
        /// <param name="cookieData">cookie</param>
        private void SignIn(string url, string formData, string cookieData)
        {

            var resultStr = DownloadString(url, formData, cookieData);

            var resultObj = resultStr.ToJsonEntity<Result>();

            // 
            var msg = string.Format("Fly社区签到信息如下:<br>签到 {0},消息: {1}", resultObj.status == 0 ? "成功" : "失败",
                resultObj.msg);
            //var msg = string.Format("Fly社区签到信息如下:<br>签到 {0},消息: {1}", "失败",
            //    "测试");
            NewLife.Log.XTrace.Log.Info(msg);

            // 发送邮件
            try
            {
                var emailCfg = ConfigHelper.GetBasicConfig().EmailCfg;
                EmailHandler.SendSmtpEMail("ismatch@qq.com", "Fly社区签到结果", msg,
                    new EmailHandler.SendAcccount
                    {
                        SmtpHost = emailCfg.SmtpHost,
                        SmtpUser = emailCfg.SmtpUser,
                        SmtpPassword = emailCfg.SmtpPassword
                    });
            }
            catch (Exception ex)
            {

            }




        }

        /// <summary>
        /// 获取签到状态
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookieData">cookie</param>
        /// <returns>是否需要签到</returns>
        private bool GetStatus(string url, string cookieData)
        {
            bool flag = false;
            var resultStr = DownloadString(url, "", cookieData);
            var resultObj = resultStr.ToJsonEntity<Result>();
            if (resultObj.status == 0)
            {
                // 赋值token
                FormData = "token="+resultObj.data.token;
                flag = resultObj.data.signed;
            }
            return flag;
        }

        /// <summary>
        /// 简单的fly社区post获取数据
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="cookieData">cookie</param>
        /// <param name="formData">附加参数</param>
        /// <returns></returns>
        private string DownloadString(string url, string formData, string cookieData)
        {
            string resultStr = "";
            var request = (HttpWebRequest)WebRequest.Create(url);

            // 定义相关请求头
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add(HttpRequestHeader.Cookie, cookieData);
            request.Referer = "http://fly.layui.com/";
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";

            // post数据相关
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // 需要传参数
            if (!formData.IsNullOrWhiteSpace())
            {
                byte[] byteData = Encoding.UTF8.GetBytes(formData);
                request.ContentLength = byteData.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteData, 0, byteData.Length);
                }
            }


            // 发出请求
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    resultStr = reader.ReadToEnd();

                }
            }
            return resultStr;
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
                public bool signed { get; set; }

                public string token { get; set; }
            }
        }
    }
}