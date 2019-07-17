using System.Diagnostics;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests
{
    //Shared test steps should be placed here
    public class BaseSteps
    {
        public string authToken;
        public Menu existingMenu;

        public BaseSteps()
        {
            Debug.WriteLine("Base Class constructor");
        }

        public void GivenIAmAUser()
        {
            //get user auth token
        }

        public void GivenIAmAnAdmin()
        {
            //get admin auth token
        }

    }
}
