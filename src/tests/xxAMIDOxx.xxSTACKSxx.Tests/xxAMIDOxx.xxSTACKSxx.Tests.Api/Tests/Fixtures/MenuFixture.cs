using xxAMIDOxx.xxSTACKSxx.Tests.Api.Configuration;
using System;
using System.Diagnostics;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures
{
    //Fixture set up and tear down
    //Inherits IDisposable as this is where teardown will take place
    public class MenuFixture : IDisposable
    {
        public ConfigModel config;
        public CrudService service;


        //Fixture set up steps go in ctor
        public MenuFixture()
        {

            //get test configuration
            config = ConfigAccessor.GetApplicationConfiguration();
            Debug.WriteLine("Fixture Constructor");

            //set-up HttpClient to be used in tests
            service = new CrudService(config.BaseUrl);
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
