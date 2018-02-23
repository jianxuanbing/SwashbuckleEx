using System;
using System.Web.Http;

namespace SwashbuckleEx.WebApiTest.Areas.Admin.Controllers
{
    /// <summary>
    /// 后台测试 相关API
    /// </summary>
    public class TestAController:ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public TestAController()
        {
            
        }

        /// <summary>
        /// 获取后台Guid
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