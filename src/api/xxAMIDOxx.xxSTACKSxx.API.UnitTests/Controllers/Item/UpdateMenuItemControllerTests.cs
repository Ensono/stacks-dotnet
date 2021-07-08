using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers
{
    [Trait("TestType", "UnitTests")]
    public class UpdateMenuItemControllerTests
    {
        [Theory, AutoData]
        public async Task UpdateMenuItem_Returns204(Guid id, Guid categoryId, Guid itemId, UpdateItemRequest body)
        {
            var controller = new UpdateMenuItemController();
            var response = await controller.UpdateMenuItem(id, categoryId, itemId, body);

            int? statusCode = ((StatusCodeResult)response).StatusCode;

            Assert.Equal(StatusCodes.Status204NoContent, statusCode);
        }
    }
}
