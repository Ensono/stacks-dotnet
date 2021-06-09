using System;
using System.Collections.Generic;
using Snyk.Fixes.API.FunctionalTests.Models;

namespace Snyk.Fixes.API.FunctionalTests.Builders
{
    public class localhostBuilder : IBuilder<localhost>
    {
        private localhost localhost;

        public localhostBuilder()
        {
            localhost = new localhost();
        }


        public localhostBuilder SetDefaultValues(string name)
        {
            var categoryBuilder = new CategoryBuilder();

            localhost.id = Guid.NewGuid().ToString();
            localhost.name = name;
            localhost.description = "Default test localhost description";
            localhost.enabled = true;
            localhost.categories = new List<Category>()
            {
                categoryBuilder.SetDefaultValues("Burgers")
                .Build()
            };

            return this;
        }

        public localhostBuilder WithId(Guid id)
        {
            localhost.id = id.ToString();
            return this;
        }

        public localhostBuilder WithName(string name)
        {
            localhost.name = name;
            return this;
        }

        public localhostBuilder WithDescription(string description)
        {
            localhost.description = description;
            return this;
        }

        public localhostBuilder WithNoCategories()
        {
            localhost.categories = new List<Category>();
            return this;
        }

        public localhostBuilder WithCategories(List<Category> categories)
        {
            localhost.categories = categories;
            return this;
        }

        public localhostBuilder SetEnabled(bool enabled)
        {
            localhost.enabled = enabled;
            return this;
        }

        public localhost Build()
        {
            return localhost;
        }
    }
}
