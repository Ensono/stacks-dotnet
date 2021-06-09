using Snyk.Fixes.API.FunctionalTests.Models;

namespace Snyk.Fixes.API.FunctionalTests.Builders
{
    public class localhostRequestBuilder : IBuilder<localhostRequest>
    {
        private localhostRequest localhost;

        public localhostRequestBuilder()
        {
            localhost = new localhostRequest();
        }

        public localhostRequestBuilder SetDefaultValues(string name)
        {
            localhost.name = name;
            localhost.description = "Updated localhost description";
            localhost.enabled = true;
            return this;
        }

        public localhostRequestBuilder WithName(string name)
        {
            localhost.name = name;
            return this;
        }

        public localhostRequestBuilder WithDescription(string description)
        {
            localhost.description = description;
            return this;
        }

        public localhostRequestBuilder SetEnabled(bool enabled)
        {
            localhost.enabled = enabled;
            return this;
        }

        public localhostRequest Build()
        {
            return localhost;
        }
    }
}
