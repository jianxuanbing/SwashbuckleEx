using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SwashbuckleEx.WebApiTest.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    public class TestController : ApiController
    {
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns></returns>
        public string Get(string id)
        {
            return id;
        }
    }
}
