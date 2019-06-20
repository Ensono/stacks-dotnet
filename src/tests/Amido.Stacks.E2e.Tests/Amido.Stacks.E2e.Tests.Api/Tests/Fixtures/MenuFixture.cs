using Amido.Stacks.E2e.Tests.Api.Configuration;
using System;
using System.Diagnostics;

namespace Amido.Stacks.E2e.Tests.Api.Tests.Fixtures
{
    //Fixture set up and tear down
    //Inherits IDisposable as this is where teardown will take place
    public class MenuFixture : IDisposable
    {
        public ConfigModel config;

        public MenuFixture()
        {
            //TFixture set up steps go here
            config = ConfigAccessor.GetApplicationConfiguration();
            Debug.WriteLine("Fixture Constructor");
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
                //I.e. Delete test users from DB
            }
        }
    }
}
