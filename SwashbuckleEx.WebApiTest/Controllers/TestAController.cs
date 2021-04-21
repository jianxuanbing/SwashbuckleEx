using System;
using System.Web.Http;

namespace SwashbuckleEx.WebApiTest.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    public class TestAController : ApiController
    {
        /// <summary>
        /// 获取通用ID
        /// </summary>
        [HttpGet]
        public Guid GetId() => Guid.NewGuid();

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="id">标识</param>
        [HttpGet]
        public string GetResult(string id) => id;
    }
}