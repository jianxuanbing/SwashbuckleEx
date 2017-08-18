using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using SwashbuckleEx.WebApiTest;

[assembly:OwinStartup(typeof(Startup))]
namespace SwashbuckleEx.WebApiTest
{

    public class Startup
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {                       
        }
    }
}