using Amido.Stacks.E2e.Tests.Api.Builders;
using Amido.Stacks.E2e.Tests.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;

namespace Amido.Stacks.E2e.Tests.Api
{
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create menus",
        SoThat = "customers know what we have on offer")]
    public class CreateMenuStory
    {
        private Menu menu;

        void GivenIHaveSpecfiedAMenuwithNoCategories()
        {
            var builder = new MenuBuilder();

            menu = builder.WithId(Guid.NewGuid())
                .WithName("TestMenu")
                .SetEnabled(true)
                .WithNoCategories()
                .Build();
        }

        void WhenICreateTheMenu()
        {

        }

        void ThenTheMenuHasBeenSuccessfullyPublished()
        {

        }
    }
}
