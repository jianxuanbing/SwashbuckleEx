using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SwashbuckleEx.WebApiTest.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    public class TestAController:ApiController
    {
        /// <summary>
        /// 获取通用ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Guid GetId()
        {
            return Guid.NewGuid();
        }
    }
}