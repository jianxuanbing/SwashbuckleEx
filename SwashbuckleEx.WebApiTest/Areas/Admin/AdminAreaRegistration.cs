using System.Web.Http;
using System.Web.Mvc;

namespace SwashbuckleEx.WebApiTest.Areas.Admin
{
    /// <summary>
    /// 后台
    /// </summary>
    public class AdminAreaRegistration:AreaRegistration
    {
        /// <summary>
        /// 注册区域
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapHttpRoute(
                "Admin_DefaultApi",
                "admin/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });
        }

        /// <summary>
        /// 区域名
        /// </summary>
        public override string AreaName
        {
            get { return "Admin"; }
        }
    }
}