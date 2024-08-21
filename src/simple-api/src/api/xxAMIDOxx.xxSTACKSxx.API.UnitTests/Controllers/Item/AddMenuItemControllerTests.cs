using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

[Trait("TestType", "UnitTests")]
public class AddMenuItemControllerTests
{
    [Theory, AutoData]
    public async Task AddMenuItem_Returns201(Guid id, Guid categoryId, CreateItemRequest body)
    {
        var controller = new AddMenuItemController();
        var response = await controller.AddMenuItem(id, categoryId, body);

        int? statusCode = ((ObjectResult)response).StatusCode;
        var content = ((ObjectResult)response).Value;

        Assert.Equal(StatusCodes.Status201Created, statusCode);
        Assert.IsType<ResourceCreatedResponse>(content);
    }
}
