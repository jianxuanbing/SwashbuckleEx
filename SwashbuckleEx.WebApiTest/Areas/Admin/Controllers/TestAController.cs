using System;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

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
        [ApiAuthor(Name = "jian玄冰",Status = DevStatus.Wait,Time = "2018-04-28")]
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}