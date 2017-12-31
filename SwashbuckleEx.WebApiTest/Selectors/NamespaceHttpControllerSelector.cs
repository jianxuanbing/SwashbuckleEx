using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace SwashbuckleEx.WebApiTest.Selectors
{
    /// <summary>
    /// 扩展自DefaultHttpControllerSelector类的控制器选择器，目前在用
    /// </summary>
    public class NamespaceHttpControllerSelector : DefaultHttpControllerSelector
    {
        private const string NamespaceRouteVariableName = "namespaces";
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<string, Type>> _apiControllerCache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public NamespaceHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
            _apiControllerCache = new Lazy<ConcurrentDictionary<string, Type>>(new Func<ConcurrentDictionary<string, Type>>(InitializeApiControllerCache));
        }
        /// <summary>
        /// 初始化ApiController实例
        /// </summary>
        /// <returns></returns>
        private ConcurrentDictionary<string, Type> InitializeApiControllerCache()
        {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            var types = this._configuration.Services.GetHttpControllerTypeResolver().GetControllerTypes(assembliesResolver).ToDictionary(t => t.FullName, t => t);
            return new ConcurrentDictionary<string, Type>(types);
        }
        /// <summary>
        /// 获取Controller全名
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerName"></param>
        public IEnumerable<string> GetControllerFullName(HttpRequestMessage request, string controllerName)
        {
            object namespaceName;
            var data = request.GetRouteData();
            IEnumerable<string> keys = _apiControllerCache.Value.ToDictionary<KeyValuePair<string, Type>, string, Type>(t => t.Key, t => t.Value, StringComparer.CurrentCultureIgnoreCase).Keys.ToList();
            if (!data.Values.TryGetValue(NamespaceRouteVariableName, out namespaceName))
            {
                return from k in keys
                       where k.EndsWith(string.Format(".{0}{1}", controllerName,
                       DefaultHttpControllerSelector.ControllerSuffix), StringComparison.CurrentCultureIgnoreCase)
                       select k;
            }
            string[] namespaces = (string[])namespaceName;
            return from n in namespaces
                   join k in keys on string.Format("{0}.{1}{2}", n, controllerName,
                   DefaultHttpControllerSelector.ControllerSuffix).ToLower() equals k.ToLower()
                   select k;
        }
        /// <summary>
        /// 获取Controller
        /// </summary>
        /// <param name="request"></param>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            Type type;
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            string controllerName = this.GetControllerName(request);
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("无法通过API路由匹配到您所请求的URI '{0}'", new object[] { request.RequestUri })));
            }
            IEnumerable<string> fullNames = GetControllerFullName(request, controllerName);
            if (fullNames.Count() == 0)
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("无法通过API路由匹配到您所请求的URI '{0}'", new object[] { request.RequestUri })));
            }
            if (this._apiControllerCache.Value.TryGetValue(fullNames.First(), out type))
            {
                return new HttpControllerDescriptor(_configuration, controllerName, type);
            }
            throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("无法通过API路由匹配到您所请求的URI '{0}'", new object[] { request.RequestUri })));
        }
        /// <summary>
        /// 获取ControllerMapping
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _apiControllerCache.Value.ToDictionary(c => c.Key, c => new HttpControllerDescriptor(_configuration, c.Value.Name.Substring(0, c.Value.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), c.Value), StringComparer.OrdinalIgnoreCase);
        }
    }
}