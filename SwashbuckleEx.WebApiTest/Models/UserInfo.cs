using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwashbuckleEx.WebApiTest.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avactor { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
}