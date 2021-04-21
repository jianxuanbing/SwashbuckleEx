using System.Web.Http;
using System.Web.Http.Dispatcher;
using SwashbuckleEx.WebApiTest.Selectors;

namespace SwashbuckleEx.WebApiTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.Services.Replace(typeof(IHttpControllerSelector),new NamespaceHttpControllerSelector(config));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "AdminApi",
                routeTemplate: "api/Admin/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional, namespaces = new string[] { "SwashbuckleEx.WebApiTest.Areas.Admin.Controllers" } });
            config.Routes.MapHttpRoute(
                name: "ClientApi",
                routeTemplate: "api/Client/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional, namespaces = new string[] { "SwashbuckleEx.WebApiTest.Areas.Client.Controllers" } });

            config.Routes.MapHttpRoute(
                name: "CommonApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional, namespaces = new string[] { "SwashbuckleEx.WebApiTest.Controllers" } }
            );

        }
    }
}
