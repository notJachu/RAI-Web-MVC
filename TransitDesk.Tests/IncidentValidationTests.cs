using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using TransitDesk.Web.Models;
using Xunit;

namespace TransitDesk.Tests;

public class IncidentValidationTests
{
    private static (bool IsValid, List<ValidationResult> Results) ValidateModel(IncidentViewModel model)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return (isValid, results);
    }

    [Fact]
    public void IncidentViewModel_TryValidateObject_SHOULD_ReturnTrue_GIVEN_ValidIncidentData()
    {
        var model = new IncidentViewModel
        {
            VehicleSideNumber = 1111,
            LineNumber = "199",
            Description = "Awaria pierwszych drzwi wejściowych podczas postoju na przystanku.",
            OccurredAt = DateTime.Now.AddHours(-1)
        };

        var (isValid, results) = ValidateModel(model);

        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Fact]
    public void IncidentViewModel_TryValidateObject_SHOULD_NOT_ReturnTrue_GIVEN_DescriptionShorterThan20Characters()
    {
        var model = new IncidentViewModel
        {
            VehicleSideNumber = 1111,
            LineNumber = "199",
            Description = "Zbyt krótki opis", 
            OccurredAt = DateTime.Now.AddHours(-1)
        };

        var (isValid, results) = ValidateModel(model);

        isValid.Should().BeFalse();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(IncidentViewModel.Description)));
    }

    [Theory]
    [InlineData("12345")] 
    [InlineData("!@#")] 
    [InlineData("")]   
    public void IncidentViewModel_TryValidateObject_SHOULD_NOT_ReturnTrue_GIVEN_InvalidLineNumber(string invalidLineNumber)
    {
        var model = new IncidentViewModel
        {
            VehicleSideNumber = 1111,
            LineNumber = invalidLineNumber,
            Description = "Prawidłowy opis zgłoszenia awarii tramwaju na trasie.",
            OccurredAt = DateTime.Now.AddHours(-1)
        };

        var (isValid, results) = ValidateModel(model);

        isValid.Should().BeFalse();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(IncidentViewModel.LineNumber)));
    }

    [Fact]
    public void IncidentViewModel_TryValidateObject_SHOULD_NOT_ReturnTrue_GIVEN_FutureOccurredAtDate()
    {
        var model = new IncidentViewModel
        {
            VehicleSideNumber = 1111,
            LineNumber = "6",
            Description = "Prawidłowy opis zgłoszenia incydentu na skrzyżowaniu.",
            OccurredAt = DateTime.Now.AddDays(1) 
        };

        var (isValid, results) = ValidateModel(model);

        isValid.Should().BeFalse();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(IncidentViewModel.OccurredAt)));
    }
}
