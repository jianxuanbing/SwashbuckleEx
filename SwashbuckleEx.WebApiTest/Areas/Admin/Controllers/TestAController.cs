using System;
using System.Web.Http;

namespace SwashbuckleEx.WebApiTest.Areas.Admin.Controllers
{
    /// <summary>
    /// 测试 相关API
    /// </summary>
    public class TestAController:ApiController
    {
        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}