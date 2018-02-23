using System;
using Swashbuckle.Swagger;

namespace Swashbuckle.Application
{
    public class InfoBuilder
    {
        private string _version;
        private string _title;
        private string _description;
        private string _termsOfService;
        /// <summary>
        /// 是否默认路由
        /// </summary>
        private bool _isDefaultRoute;
        private readonly ContactBuilder _contactBuilder = new ContactBuilder();
        private readonly LicenseBuilder _licenseBuilder = new LicenseBuilder();

        public InfoBuilder(string version, string title)
        {
            _version = version;
            _title = title;
        }

        public InfoBuilder(string version, string title, bool isDefualt):this(version,title)
        {
            _isDefaultRoute = isDefualt;
        }

        public InfoBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        public InfoBuilder TermsOfService(string termsOfService)
        {
            _termsOfService = termsOfService;
            return this;
        }

        public InfoBuilder Contact(Action<ContactBuilder> contact)
        {
            contact(_contactBuilder);
            return this;
        }

        public InfoBuilder License(Action<LicenseBuilder> license)
        {
            license(_licenseBuilder);
            return this;
        }

        internal Info Build()
        {
            return new Info
            {
                version = _version,
                title = _title,
                description = _description,
                termsOfService = _termsOfService,
                contact = _contactBuilder.Build(),
                license = _licenseBuilder.Build(),
                isDefaultRoute = _isDefaultRoute
            };
        }
    }
}