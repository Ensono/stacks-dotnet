using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Controllers;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class ApiControllerBaseTests
{
    [Fact]
    public void GetCorrelationId_ReturnsGuid_WhenCorrelationIdIsProvided()
    {
        var controller = new ApiControllerBase { ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.HttpContext.Request.Headers["x-correlation-id"] = "d290f1ee-6c54-4b01-90e6-d701748f0851";

        var result = controller.GetCorrelationId();

        Assert.Equal(Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"), result);
    }

    [Fact]
    public void GetCorrelationId_ThrowsArgumentException_WhenCorrelationIdIsNotProvided()
    {
        var controller = new ApiControllerBase { ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var exception = Assert.Throws<ArgumentException>(() => controller.GetCorrelationId());

        Assert.Equal("The correlation id couldn't be loaded", exception.Message);
    }

    [Fact]
    public void GetCorrelationId_ThrowsFormatException_WhenCorrelationIdIsInvalid()
    {
        var controller = new ApiControllerBase { ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.HttpContext.Request.Headers["x-correlation-id"] = "invalid-guid";

        Assert.Throws<FormatException>(() => controller.GetCorrelationId());
    }
}
