using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

[Trait("TestType", "UnitTests")]
public class DeleteMenuItemControllerTests
{
    [Theory, AutoData]
    public async Task DeleteMenuItem_Returns204(Guid id, Guid categoryId, Guid itemId)
    {
        var controller = new DeleteMenuItemController();
        var response = await controller.DeleteMenuItem(id, categoryId, itemId);

        int? statusCode = ((StatusCodeResult)response).StatusCode;

        Assert.Equal(StatusCodes.Status204NoContent, statusCode);
    }
}
