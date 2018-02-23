using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Application;
using SwashbuckleEx.WebApiTest;
using SwashbuckleEx.WebApiTest.Selectors;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace SwashbuckleEx.WebApiTest
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    //c.SingleApiVersion("v1", "Test.WebApi");                    
                    c.MultipleApiVersions(ResolveAreasSupportByRouteConstraint, (vc) =>
                    {
                        vc.Version("Admin", "中文后台 API").Description("这个用于测试一下备注信息").TermsOfService("www.baidu.com").License(
                            x =>
                            {
                                x.Name("jian玄冰");
                                x.Url("www.baidu.2333");
                            })
                            .Contact(x =>
                            {
                                x.Name("2017").Email("jianxuanhuo1@126.com").Url("www.baidu.xxxx");
                            });
                        vc.Version("v1", "Common API",true);
                        
                        vc.Version("Client", "Client API");                        
                    });
                    c.ApiKey("Authorization").Description("OAuth2 Auth").In("header").Name("Authorization");
                    //c.OAuth2("jwt").AuthorizationUrl("http://localhost:9460/oauth/token")
                    //    .TokenUrl("http://localhost:9460/oauth/token").Scopes(
                    //        x =>
                    //        {
                    //            x.Add("scope", "admin");
                    //        });
                    c.DocumentFilter<SwaggerAreasSupportDocumentFilter>();
                    c.IncludeXmlComments(string.Format("{0}/bin/SwashbuckleEx.WebApiTest.XML", AppDomain.CurrentDomain.BaseDirectory));
                })
                .EnableSwaggerUi(c =>
                {
                    c.EnableApiKeySupport("Authorization","header");
                });
        }

        private static bool ResolveAreasSupportByRouteConstraint(ApiDescription apiDescription, string targetApiVersion)
        {
            if (targetApiVersion == "v1")
            {
                return apiDescription.Route.RouteTemplate.StartsWith("api/{controller}");
            }
            var routeTemplateStart = "api/" + targetApiVersion;
            return apiDescription.Route.RouteTemplate.StartsWith(routeTemplateStart);
        }
    }
}