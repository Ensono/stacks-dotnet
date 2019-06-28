using Amido.Stacks.E2e.Tests.Api.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Tests.Fixtures
{
    //Inherits IDisposable as this is where teardown will take place
    public class MenuFixture : IDisposable
    {
        public ConfigModel configuration;

        public MenuFixture()
        {
            //TFixture set up steps go here
            configuration = ConfigAccessor.GetApplicationConfiguration();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                //Fixture teardown steps go here
            }
        }
    }
}
