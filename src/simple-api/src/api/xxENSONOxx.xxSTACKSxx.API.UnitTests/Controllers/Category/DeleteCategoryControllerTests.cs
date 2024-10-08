﻿using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Controllers;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Controllers.Category;

[Trait("TestType", "UnitTests")]
public class DeleteCategoryControllerTests
{
    [Theory, AutoData]
    public async Task DeleteCategory_Returns204(Guid id, Guid categoryId)
    {
        var controller = new DeleteCategoryController();
        var response = await controller.DeleteCategory(id, categoryId);

        int? statusCode = ((StatusCodeResult)response).StatusCode;

        Assert.Equal(StatusCodes.Status204NoContent, statusCode);
    }
}
