using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers
{
    [Trait("TestType", "UnitTests")]
    public class AddMenuCategoryControllerTests
    {
        [Theory, AutoData]
        public async Task AddMenuCategory_Returns201(Guid id, CreateCategoryRequest body)
        {
            var controller = new AddMenuCategoryController();
            var response = await controller.AddMenuCategory(id, body);

            int? statusCode = ((ObjectResult)response).StatusCode;
            var content = ((ObjectResult)response).Value;

            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.IsType<ResourceCreatedResponse>(content);
        }
    }
}
