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
                        vc.Version("Admin", "Admin API");
                        vc.Version("v1", "Common API");
                        
                        vc.Version("Client", "Client API");                        
                    });
                    c.ApiKey("Authorization").Description("OAuth2 Auth").In("header").Name("Bearer ");
                    c.DocumentFilter<SwaggerAreasSupportDocumentFilter>();
                    c.IncludeXmlComments(string.Format("{0}/bin/SwashbuckleEx.WebApiTest.XML", AppDomain.CurrentDomain.BaseDirectory));
                })
                .EnableSwaggerUi(c =>
                {
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