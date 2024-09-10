using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Controllers;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Controllers.Menu;

[Trait("TestType", "UnitTests")]
public class DeleteMenuControllerTests
{
    [Theory, AutoData]
    public async Task DeleteMenu_Returns204(Guid id)
    {
        var controller = new DeleteMenuController();
        var response = await controller.DeleteMenu(id);

        int? statusCode = ((StatusCodeResult)response).StatusCode;

        Assert.Equal(StatusCodes.Status204NoContent, statusCode);
    }
}
