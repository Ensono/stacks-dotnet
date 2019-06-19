using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api
{
    //Inherits IDisposable as this is where teardown will take place
    public class MenuFixture : IDisposable
    {
        public AppConfig configuration;

        public MenuFixture()
        {
            configuration = Configuration.GetApplicationConfiguration();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                //teardown goes here
            }
        }
    }
}
