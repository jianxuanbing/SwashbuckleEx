using System.Web.Http;
using System.Web.Mvc;

namespace SwashbuckleEx.WebApiTest.Areas.Client
{
    /// <summary>
    /// 客户端 区域注册
    /// </summary>
    public class ClientAreaRegistration: AreaRegistration
    {
        /// <summary>
        /// 注册区域
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                "ClientArea_DefaultApi", 
                "client/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional});
        }

        /// <summary>
        /// 区域名
        /// </summary>
        public override string AreaName
        {
            get { return "Client"; }
        }
    }
}