using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using SwashbuckleEx.WebApiTest.Extensions;
using SwashbuckleEx.WebApiTest.Models;

namespace SwashbuckleEx.WebApiTest.Areas.Admin.Controllers
{
    /// <summary>
    /// 后台测试 相关API
    /// </summary>
    public class TestAController : ApiController
    {
        /// <summary>
        /// 获取后台Guid
        /// </summary>
        /// <remarks>
        /// 测试一些内容，不想将无用的东西放在接口名称当中<br/>
        /// 换行输出一下内容
        /// </remarks>
        [HttpGet]
        public Guid GetGuid() => Guid.NewGuid();

        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpPost]
        [Upload]
        public void UploadFile()
        {
        }

        /// <summary>
        /// 查看API开发状态
        /// </summary>
        [HttpGet]
        [ApiAuthor(Name = "jian玄冰", Status = DevStatus.Wait, Time = "2018-04-28")]
        public void ApiStatus()
        {
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "自定义内容", Type = typeof(UserInfo))]
        public HttpResponseMessage GetUserInfo() => Request.CreateResponse(HttpStatusCode.OK, new UserInfo(), "application/json");
    }
}