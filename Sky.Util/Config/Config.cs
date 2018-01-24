using System;
using System.Collections.Generic;
using System.Text;

namespace MyTest.Config
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class Config
    {
        public EmailConfig EmailCfg { get; set; }

        public FlyAccountConfig FlyCfg { get; set; }

        /// <summary>
        /// 邮箱信息
        /// </summary>
        public class EmailConfig
        {
            /// <summary> SMTP服务器地址 </summary>
            public string SmtpHost { get; set; }

            /// <summary> 登录账号 </summary>
            public string SmtpUser { get; set; }

            /// <summary> 登录密码 </summary>
            public string SmtpPassword { get; set; }
        }

        /// <summary>
        /// Fly社区信息
        /// </summary>
        public class FlyAccountConfig
        {
            public string CookieData { get; set; }
        }
    }
}
