using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Fakes;
using Microsoft.Extensions.Configuration;

namespace Dhgms.AspNetCoreContrib.Example.WebSite
{
    public class Startup : Dhgms.AspNetCoreContrib.App.BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override Assembly[] GetControllerAssemblies()
        {
            return new[]
            {
                typeof(FakeCrudController).Assembly,
            };
        }

        protected override Assembly[] GetMediatrAssemblies()
        {
            return new[]
            {
                typeof(FakeCrudController).Assembly,
                typeof(Startup).Assembly,
            };
        }
    }
}
