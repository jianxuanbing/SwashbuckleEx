using System.Linq;
using System.Collections.Generic;
using System.Web.Http.Description;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Xml.XPath;
using Swashbuckle.Application;
using Swashbuckle.Swagger.XmlComments;

namespace Swashbuckle.Swagger
{
    public class SwaggerGenerator : ISwaggerProvider
    {
        private readonly IApiExplorer _apiExplorer;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly IDictionary<string, Info> _apiVersions;
        private readonly SwaggerGeneratorOptions _options;

        public SwaggerGenerator(
            IApiExplorer apiExplorer,
            JsonSerializerSettings jsonSerializerSettings,
            IDictionary<string, Info> apiVersions,
            SwaggerGeneratorOptions options = null)
        {
            _apiExplorer = apiExplorer;
            _jsonSerializerSettings = jsonSerializerSettings;
            _apiVersions = apiVersions;
            _options = options ?? new SwaggerGeneratorOptions();
        }

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var schemaRegistry = new SchemaRegistry(
                _jsonSerializerSettings,
                _options.CustomSchemaMappings,
                _options.SchemaFilters,
                _options.ModelFilters,
                _options.IgnoreObsoleteProperties,
                _options.SchemaIdSelector,
                _options.DescribeAllEnumsAsStrings,
                _options.DescribeStringEnumsInCamelCase,
                _options.ApplyFiltersToAllSchemas);

            Info info;
            _apiVersions.TryGetValue(apiVersion, out info);
            if (info == null)
                throw new UnknownApiVersion(apiVersion);


            var paths = GetApiDescriptionsFor(apiVersion)
                .Where(apiDesc => !(_options.IgnoreObsoleteActions && apiDesc.IsObsolete()))
                .OrderBy(_options.GroupingKeySelector, _options.GroupingKeyComparer)
                .GroupBy(apiDesc => apiDesc.RelativePathSansQueryString())
                .ToDictionary(group => "/" + group.Key, group => CreatePathItem(group, schemaRegistry));

            var rootUri = new Uri(rootUrl);
            var port = (!rootUri.IsDefaultPort) ? ":" + rootUri.Port : string.Empty;

            var swaggerDoc = new SwaggerDocument
            {
                info = info,
                host = rootUri.Host + port,
                basePath = (rootUri.AbsolutePath != "/") ? rootUri.AbsolutePath : null,
                schemes = (_options.Schemes != null) ? _options.Schemes.ToList() : new[] { rootUri.Scheme }.ToList(),
                paths = paths,
                definitions = schemaRegistry.Definitions,
                securityDefinitions = _options.SecurityDefinitions
            };

            swaggerDoc.muiltVersion = new List<Info>();
            foreach (var version in _apiVersions)
            {
                swaggerDoc.muiltVersion.Add(version.Value);
            }

            if (SwaggerEnabledConfiguration.DiscoveryPaths != null &&
                SwaggerEnabledConfiguration.DiscoveryPaths.Any())
            {

                foreach (var version in swaggerDoc.muiltVersion)
                {
                    var path = SwaggerEnabledConfiguration.DiscoveryPaths.FirstOrDefault(x => x.Contains(version.version));
                    if (!string.IsNullOrEmpty(path))
                    {
                        version.docPath = path;
                        if (version.version == swaggerDoc.info.version)
                        {
                            swaggerDoc.info.docPath = path;
                        }
                    }
                }
            }

            var keys = paths.Keys.ToList();
            SetTags(swaggerDoc, _options.ModelFilters, keys);

            foreach (var filter in _options.DocumentFilters.OrderBy(p=>p))
            {
                filter.Apply(swaggerDoc, schemaRegistry, _apiExplorer);
            }
            //todo 这里是排序的代码
            //mt 2019-01-04
            swaggerDoc.paths = swaggerDoc.paths.OrderBy(p=>p.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            return swaggerDoc;
        }

        /// <summary>
        /// 设置标签
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="modelFilters"></param>
        /// <param name="keys"></param>
        private void SetTags(SwaggerDocument swaggerDoc, IEnumerable<IModelFilter> modelFilters, List<string> keys)
        {
            var result = new List<Tag>();
            var isKeys = keys != null && keys.Count > 0;
            try
            {
                string memberXPath = "/doc/members/member[starts-with(@name,'T:')]";
                foreach (var modelFilter in modelFilters)
                {
                    if (modelFilter.XmlNavigator == null)
                    {
                        continue;
                    }
                    var typeNode = modelFilter.XmlNavigator.Select(memberXPath);
                    foreach (XPathNavigator item in typeNode)
                    {
                        var summaryNode = item.SelectSingleNode("summary");
                        if (summaryNode != null)
                        {
                            var name = item.GetAttribute("name", "");
                            if (name.EndsWith("DeptController"))
                            {

                            }
                            if (!swaggerDoc.info.isDefaultRoute)
                            {
                                //第二个判断改了下
                                //mt 2019-01-04
                                if (!name.Contains("Areas") || !name.EndsWith("Controller"))
                                {
                                    continue;
                                }

                                var names = name.Split('.');
                                var areaIndex = names.ToList().IndexOf("Areas") + 1;
                                if (!names[areaIndex].Equals(swaggerDoc.info.version,
                                    StringComparison.CurrentCultureIgnoreCase))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (name.Contains("Areas"))
                                {
                                    continue;
                                }
                            }
                            var nameXPath = "/doc/members/member[starts-with(@name,'M:" + name.Replace("T:", "") + "')]";
                            var nameXPathNode = modelFilter.XmlNavigator.Select(nameXPath);

                            // 处理构造函数导致接口数量不正确问题
                            var ctorXPath = "/doc/members/member[starts-with(@name,'M:" + name.Replace("T:", "") +
                                            ".#ctor')]";
                            var ctorXPathNode = modelFilter.XmlNavigator.Select(ctorXPath);

                            if (name.Contains("Controllers") || (name.EndsWith("Controller")))
                            {

                                name = name.Split('.').Last().Replace("Controller", "");

                                var summary = summaryNode.ExtractContent();

                                if (nameXPathNode.Count > 0)
                                {
                                    summary = ctorXPathNode.Count > 0
                                        ? summary + "(" + (nameXPathNode.Count - ctorXPathNode.Count) + ")"
                                        : summary + "(" + nameXPathNode.Count + ")";
                                }
                                result.Add(new Tag() { name = name, description = summary });
                            }
                            // 只处理控制器注释标签
                            //name = name.Split('.').Last().Replace("Controller", "");

                            //var summary = summaryNode.ExtractContent();
                            ////if (isKeys)
                            ////{
                            ////    var count = keys.Count(r => r.StartsWith("/" + name + "/"));
                            ////    summary = summary + "(" + count + ")";
                            ////}


                            //if (nameXPathNode.Count > 0)
                            //{
                            //    summary = ctorXPathNode.Count > 0
                            //        ? summary + "(" + (nameXPathNode.Count - ctorXPathNode.Count) + ")"
                            //        : summary + "(" + nameXPathNode.Count + ")";
                            //}
                            //result.Add(new Tag() { name = name, description = summary });
                        }
                    }
                }
            }
            catch (Exception)
            {
                //忽略                
            }
            swaggerDoc.tags = result;
        }

        private IEnumerable<ApiDescription> GetApiDescriptionsFor(string apiVersion)
        {
            return (_options.VersionSupportResolver == null)
                ? _apiExplorer.ApiDescriptions
                : _apiExplorer.ApiDescriptions.Where(apiDesc => _options.VersionSupportResolver(apiDesc, apiVersion));
        }

        private PathItem CreatePathItem(IEnumerable<ApiDescription> apiDescriptions, SchemaRegistry schemaRegistry)
        {
            var pathItem = new PathItem();

            // Group further by http method
            var perMethodGrouping = apiDescriptions
                .GroupBy(apiDesc => apiDesc.HttpMethod.Method.ToLower());

            foreach (var group in perMethodGrouping)
            {
                var httpMethod = group.Key;

                var apiDescription = (group.Count() == 1)
                    ? group.First()
                    : _options.ConflictingActionsResolver(group);

                switch (httpMethod)
                {
                    case "get":
                        pathItem.get = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "put":
                        pathItem.put = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "post":
                        pathItem.post = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "delete":
                        pathItem.delete = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "options":
                        pathItem.options = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "head":
                        pathItem.head = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "patch":
                        pathItem.patch = CreateOperation(apiDescription, schemaRegistry);
                        break;
                }
            }

            return pathItem;
        }

        private Operation CreateOperation(ApiDescription apiDesc, SchemaRegistry schemaRegistry)
        {
            var parameters = apiDesc.ParameterDescriptions
                .Select(paramDesc =>
                    {
                        string location = GetParameterLocation(apiDesc, paramDesc);
                        return CreateParameter(location, paramDesc, schemaRegistry);
                    })
                 .ToList();

            var responses = new Dictionary<string, Response>();
            var responseType = apiDesc.ResponseType();
            if (responseType == null || responseType == typeof(void))
                responses.Add("204", new Response { description = "No Content" });
            else
                responses.Add("200", new Response { description = "OK", schema = schemaRegistry.GetOrRegister(responseType) });

            var operation = new Operation
            {
                tags = new[] { _options.GroupingKeySelector(apiDesc) },
                operationId = apiDesc.FriendlyId(),
                produces = apiDesc.Produces().ToList(),
                consumes = apiDesc.Consumes().ToList(),
                parameters = parameters.Any() ? parameters : null, // parameters can be null but not empty
                responses = responses,
                deprecated = apiDesc.IsObsolete() ? true : (bool?)null
            };

            foreach (var filter in _options.OperationFilters)
            {
                filter.Apply(operation, schemaRegistry, apiDesc);
            }

            return operation;
        }

        private string GetParameterLocation(ApiDescription apiDesc, ApiParameterDescription paramDesc)
        {
            if (apiDesc.RelativePathSansQueryString().Contains("{" + paramDesc.Name + "}"))
                return "path";
            else if (paramDesc.Source == ApiParameterSource.FromBody && apiDesc.HttpMethod != HttpMethod.Get)
                return "body";
            else
                return "query";
        }

        private Parameter CreateParameter(string location, ApiParameterDescription paramDesc, SchemaRegistry schemaRegistry)
        {
            var parameter = new Parameter
            {
                @in = location,
                name = paramDesc.Name
            };

            if (paramDesc.ParameterDescriptor == null)
            {
                parameter.type = "string";
                parameter.required = true;
                return parameter;
            }

            parameter.required = location == "path" || !paramDesc.ParameterDescriptor.IsOptional;
            parameter.@default = paramDesc.ParameterDescriptor.DefaultValue;

            var schema = schemaRegistry.GetOrRegister(paramDesc.ParameterDescriptor.ParameterType);
            if (parameter.@in == "body")
                parameter.schema = schema;
            else
                parameter.PopulateFrom(schema);

            return parameter;
        }
    }
}
