﻿using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

[Trait("TestType", "UnitTests")]
public class SearchMenuControllerTests
{
    [Theory, AutoData]
    public async Task SearchMenu_Returns200_AndSearchMenuResponse(string searchTerm, Guid? restaurantId, int pageSize, int pageNumber)
    {
        var controller = new SearchMenuController();
        var response = await controller.SearchMenu(searchTerm, restaurantId, pageSize, pageNumber);

        var content = ((ObjectResult)response).Value;

        Assert.IsType<SearchMenuResponse>(content);
        var offset = ((SearchMenuResponse)content).Offset;
        var size = ((SearchMenuResponse)content).Size;
        Assert.Equal(pageSize, size);
        Assert.Equal(pageNumber * pageSize, offset);
    }
}
