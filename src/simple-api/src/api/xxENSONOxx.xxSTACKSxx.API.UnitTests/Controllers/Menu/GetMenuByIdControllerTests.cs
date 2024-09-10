using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Controllers;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Controllers.Menu;

[Trait("TestType", "UnitTests")]
public class GetMenuByIdControllerTests
{
    [Theory, AutoData]
    public async Task GetMenu_Returns200_AndMenu(Guid id)
    {
        var controller = new GetMenuByIdController();
        var response = await controller.GetMenu(id);

        var content = ((ObjectResult)response).Value;
        Assert.IsType<Models.Responses.Menu>(content);
    }
}
