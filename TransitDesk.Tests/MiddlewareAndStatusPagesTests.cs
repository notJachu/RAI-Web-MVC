using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitDesk.Web.Controllers;
using Xunit;

namespace TransitDesk.Tests;

public class MiddlewareAndStatusPagesTests
{
    private readonly ErrorsController _controller;

    public MiddlewareAndStatusPagesTests()
    {
        _controller = new ErrorsController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public void ErrorsController_Status_SHOULD_ReturnNotFoundViewWithStatusCode404_GIVEN_StatusCode404()
    {
        const int statusCode = 404;

        var result = _controller.Status(statusCode);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewName.Should().Be("NotFound");
        _controller.Response.StatusCode.Should().Be(404);
        ((int)_controller.ViewBag.StatusCode).Should().Be(404);
    }

    [Fact]
    public void ErrorsController_Status_SHOULD_ReturnForbiddenViewWithStatusCode403_GIVEN_StatusCode403()
    {
        const int statusCode = 403;

        var result = _controller.Status(statusCode);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewName.Should().Be("Forbidden");
        _controller.Response.StatusCode.Should().Be(403);
        ((int)_controller.ViewBag.StatusCode).Should().Be(403);
    }

    [Theory]
    [InlineData(500)]
    [InlineData(502)]
    public void ErrorsController_Status_SHOULD_ReturnDefaultViewWithStatusCode_GIVEN_UnhandledStatusCode(int unhandledCode)
    {
        var result = _controller.Status(unhandledCode);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewName.Should().Be("Default");
        _controller.Response.StatusCode.Should().Be(unhandledCode);
        ((int)_controller.ViewBag.StatusCode).Should().Be(unhandledCode);
    }
}
