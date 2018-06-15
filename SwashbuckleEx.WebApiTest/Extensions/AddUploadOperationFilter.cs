using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace SwashbuckleEx.WebApiTest.Extensions
{
    /// <summary>
    /// 添加 上传操作过滤
    /// </summary>
    public class AddUploadOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 重写Apply方法，加入Upload操作过滤
        /// </summary>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var upload = apiDescription.ActionDescriptor.GetCustomAttributes<UploadAttribute>().FirstOrDefault();
            if (upload == null)
            {
                return;
            }
            operation.consumes.Add("application/form-data");
            if (operation.parameters==null)
            {
                operation.parameters = new List<Parameter>();
            }
            operation.parameters.Add(new Parameter()
            {
                name = upload.Name,
                @in = "formData",
                required = upload.Require,
                type = "file",
                description = upload.Description
            });
        }
    }
}