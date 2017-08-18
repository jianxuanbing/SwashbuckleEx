using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Swashbuckle.Application;
using SwashbuckleEx.WebApiTest;

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
                    c.SingleApiVersion("v1", "Test.WebApi");
                    c.ApiKey("Authorization").Description("OAuth2 Auth").In("header").Name("Bearer ");
                    c.IncludeXmlComments(string.Format("{0}/bin/SwashbuckleEx.WebApiTest.XML", AppDomain.CurrentDomain.BaseDirectory));
                })
                .EnableSwaggerUi(c =>
                {
                });
        }
    }
}