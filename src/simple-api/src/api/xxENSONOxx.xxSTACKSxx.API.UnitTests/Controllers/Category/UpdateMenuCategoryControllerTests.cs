using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Controllers;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Controllers.Category;

[Trait("TestType", "UnitTests")]
public class UpdateMenuCategoryControllerTests
{
    [Theory, AutoData]
    public async Task UpdateMenuCategory_Returns204(Guid id, Guid categoryId, UpdateCategoryRequest body)
    {
        var controller = new UpdateMenuCategoryController();
        var response = await controller.UpdateMenuCategory(id, categoryId, body);

        int? statusCode = ((StatusCodeResult)response).StatusCode;

        Assert.Equal(StatusCodes.Status204NoContent, statusCode);
    }
}
