using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swashbuckle.Swagger.Annotations
{
    /// <summary>
    /// Api作者信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiAuthorAttribute:Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 开发状态
        /// </summary>
        public DevStatus Status { get; set; }

        public string GetStatusName()
        {
            switch (Status)
            {
                case DevStatus.Wait:
                    return "等待开发";
                case DevStatus.Dev:
                    return "开发中";
                case DevStatus.Finish:
                    return "开发完成";
                default:
                    return string.Empty;
            }
        }
    }

    /// <summary>
    /// 开发状态
    /// </summary>
    public enum DevStatus
    {
        /// <summary>
        /// 等待开发
        /// </summary>
        Wait,
        /// <summary>
        /// 开发中
        /// </summary>
        Dev,
        /// <summary>
        /// 开发完成
        /// </summary>
        Finish
    }
}
