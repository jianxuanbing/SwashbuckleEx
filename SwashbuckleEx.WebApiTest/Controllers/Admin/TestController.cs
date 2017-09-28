using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SwashbuckleEx.WebApiTest.Controllers.Admin
{
    public class TestController:ApiController
    {
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Guid GetTestId()
        {
            return Guid.NewGuid();
        }
    }
}