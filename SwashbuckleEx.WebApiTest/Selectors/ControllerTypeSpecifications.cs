using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Dispatcher;

namespace SwashbuckleEx.WebApiTest.Selectors
{
    /// <summary>
    /// 控制器类型规范 扩展
    /// </summary>
    internal static class ControllerTypeSpecifications
    {
        /// <summary>
        /// 根据区域名获取键值对
        /// </summary>
        /// <param name="query">键值对列表</param>
        /// <param name="areaName">区域名</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, Type>> ByAreaName(
            this IEnumerable<KeyValuePair<string, Type>> query, string areaName)
        {
            string areaNameToFind = string.Format(CultureInfo.CurrentCulture, ".{0}.", areaName);
            return query.Where(x => x.Key.IndexOf(areaNameToFind, StringComparison.OrdinalIgnoreCase) != -1);
        }

        /// <summary>
        /// 获取不包含区域名的键值对
        /// </summary>
        /// <param name="query">键值对列表</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, Type>> WithoutAreaName(
            this IEnumerable<KeyValuePair<string, Type>> query)
        {
            return query.Where(x => x.Key.IndexOf(".areas.", StringComparison.OrdinalIgnoreCase) == -1);
        }

        /// <summary>
        /// 根据控制器名获取键值对
        /// </summary>
        /// <param name="query">键值对列表</param>
        /// <param name="controllerName">控制器名</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, Type>> ByControllerName(
            this IEnumerable<KeyValuePair<string, Type>> query, string controllerName)
        {
            string controllerNameToFind = string.Format(CultureInfo.InvariantCulture, ".{0}{1}", controllerName,
                DefaultHttpControllerSelector.ControllerSuffix);
            return query.Where(x => x.Key.EndsWith(controllerNameToFind, StringComparison.OrdinalIgnoreCase));
        }
    }
}