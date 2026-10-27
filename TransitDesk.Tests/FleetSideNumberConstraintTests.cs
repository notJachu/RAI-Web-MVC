using System.Globalization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TransitDesk.Web.Constraints;
using Xunit;

namespace TransitDesk.Tests;

public class FleetSideNumberConstraintTests
{
    private readonly FleetSideNumberConstraint _constraint = new();

    [Theory]
    [InlineData(1000)]
    [InlineData(2072)]
    [InlineData(9999)]
    public void FleetSideNumberConstraint_Match_SHOULD_ReturnTrue_GIVEN_ValidSideNumber(int sideNumber)
    {
        var values = new RouteValueDictionary
        {
            ["sideNumber"] = sideNumber
        };

        var result = _constraint.Match(
            httpContext: null,
            route: null,
            routeKey: "sideNumber",
            values: values,
            routeDirection: RouteDirection.IncomingRequest);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(999)]
    [InlineData(10000)]
    [InlineData("abc")]
    [InlineData(null)]
    public void FleetSideNumberConstraint_Match_SHOULD_NOT_ReturnTrue_GIVEN_InvalidSideNumber(object? invalidSideNumber)
    {
        var values = new RouteValueDictionary
        {
            ["sideNumber"] = invalidSideNumber
        };

        var result = _constraint.Match(
            httpContext: null,
            route: null,
            routeKey: "sideNumber",
            values: values,
            routeDirection: RouteDirection.IncomingRequest);

        result.Should().BeFalse();
    }

    [Fact]
    public void FleetSideNumberConstraint_Match_SHOULD_NOT_ReturnTrue_GIVEN_MissingRouteKey()
    {
        var values = new RouteValueDictionary();

        var result = _constraint.Match(
            httpContext: null,
            route: null,
            routeKey: "sideNumber",
            values: values,
            routeDirection: RouteDirection.IncomingRequest);

        result.Should().BeFalse();
    }
}
