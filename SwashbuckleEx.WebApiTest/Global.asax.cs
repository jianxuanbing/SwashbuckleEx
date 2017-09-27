using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using SwashbuckleEx.WebApiTest.Selectors;

namespace SwashbuckleEx.WebApiTest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // WebApi支持Area
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
        }
    }
}
