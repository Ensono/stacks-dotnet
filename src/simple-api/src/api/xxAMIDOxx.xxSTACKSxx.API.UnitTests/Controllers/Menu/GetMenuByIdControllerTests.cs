using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

[Trait("TestType", "UnitTests")]
public class GetMenuByIdControllerTests
{
    [Theory, AutoData]
    public async Task GetMenu_Returns200_AndMenu(Guid id)
    {
        var controller = new GetMenuByIdController();
        var response = await controller.GetMenu(id);

        var content = ((ObjectResult)response).Value;
        Assert.IsType<Menu>(content);
    }
}
