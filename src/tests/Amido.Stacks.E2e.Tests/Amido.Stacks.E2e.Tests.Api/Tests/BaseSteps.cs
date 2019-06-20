using System.Diagnostics;

namespace Amido.Stacks.E2e.Tests.Api.Tests
{
    //Shared test steps should be placed here
    public class BaseStory
    {
        public string authToken;

        public BaseStory()
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
