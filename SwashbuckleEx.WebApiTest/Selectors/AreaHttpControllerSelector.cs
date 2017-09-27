using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace SwashbuckleEx.WebApiTest.Selectors
{
    /// <summary>
    /// WebApi区域控制器选择器
    /// </summary>
    public class AreaHttpControllerSelector : DefaultHttpControllerSelector
    {
        /// <summary>
        /// 区域路由变量名
        /// </summary>
        private const string AreaRouteVariableName = "area";

        /// <summary>
        /// Http配置
        /// </summary>
        private readonly HttpConfiguration _configuration;

        /// <summary>
        /// Api控制器类型字典
        /// </summary>
        private readonly Lazy<ConcurrentDictionary<string, Type>> _apiControllerTypes;

        /// <summary>
        /// 初始化一个<see cref="AreaHttpControllerSelector"/>类型的实例
        /// </summary>
        /// <param name="configuration">Http配置</param>
        public AreaHttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _apiControllerTypes = new Lazy<ConcurrentDictionary<string, Type>>(GetControllerTypes);
        }

        /// <summary>
        /// 获取控制器类型字典
        /// </summary>
        /// <returns></returns>
        private static ConcurrentDictionary<string, Type> GetControllerTypes()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Dictionary<string, Type> types = assemblies
                .SelectMany(a => a
                    .GetTypes().Where(t =>
                        !t.IsAbstract &&
                        t.Name.EndsWith(ControllerSuffix, StringComparison.OrdinalIgnoreCase) &&
                        typeof(IHttpController).IsAssignableFrom(t)))
                .ToDictionary(t => t.FullName, t => t);
            return new ConcurrentDictionary<string, Type>(types);
        }

        /// <summary>
        /// 由Http请求获取控制台描述信息
        /// </summary>
        /// <param name="request">Http请求消息</param>
        /// <returns></returns>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            return GetApiController(request);
        }

        /// <summary>
        /// 获取Api控制器
        /// </summary>
        /// <param name="request">Http请求消息</param>
        /// <returns></returns>
        private HttpControllerDescriptor GetApiController(HttpRequestMessage request)
        {
            string areaName = GetAreaName(request);
            string controllerName = GetControllerName(request);
            if (controllerName == null)
            {
                throw new InvalidOperationException("获取的Api控制器名称为空");
            }
            Type type = GetControllerType(areaName, controllerName);
            return new HttpControllerDescriptor(_configuration, controllerName, type);
        }

        /// <summary>
        /// 获取区域名
        /// </summary>
        /// <param name="request">Http请求消息</param>
        /// <returns></returns>
        private static string GetAreaName(HttpRequestMessage request)
        {
            IHttpRouteData data = request.GetRouteData();
            object areaName;
            if (data.Route == null || data.Route.DataTokens == null)
            {
                if (data.Values.TryGetValue(AreaRouteVariableName, out areaName))
                {
                    return areaName.ToString();
                }
                return null;
            }
            return data.Route.DataTokens.TryGetValue(AreaRouteVariableName, out areaName) ? areaName.ToString() : null;
        }

        /// <summary>
        /// 获取控制器类型
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="controllerName">控制器名</param>
        /// <returns></returns>
        private Type GetControllerType(string areaName, string controllerName)
        {
            IEnumerable<KeyValuePair<string, Type>> query = _apiControllerTypes.Value.AsEnumerable();
            query = string.IsNullOrWhiteSpace(areaName) ? query.WithoutAreaName() : query.ByAreaName(areaName);
            Type type = query.ByControllerName(controllerName).Select(m => m.Value).SingleOrDefault();
            if (type == null)
            {
                throw new Exception(string.Format("未找到名称为“{0}”的Api控制器",controllerName));
            }
            return type;
        }
    }
}