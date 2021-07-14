using System;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures
{
    public class AuthFixture : IDisposable
    {
        public void GivenAUser()
        {
            //todo: implement givenauser
        }

        public void GivenAnAdmin()
        {
            //ToDo: implement givenanadmin
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //fixture tear down goes here
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
