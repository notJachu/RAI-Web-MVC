using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TransitDesk.Web.Validators;

namespace TransitDesk.Web.Models;

public class IncidentViewModel
{
    [Required(ErrorMessage = "Numer taborowy jest wymagany.")]
    [Remote(action: "VerifyVehicleAvailable", controller: "Incidents", ErrorMessage = "Pojazd o podanym numerze nie istnieje lub jest wycofany.")]
    [Display(Name = "Numer taborowy pojazdu")]
    public int VehicleSideNumber { get; set; }
    
    [Required(ErrorMessage = "Pole jest wymagane.")]
    [RegularExpression(@"^[a-zA-Z0-9]{1,4}$", ErrorMessage = "Kod musi składać się z 1 do 4 znaków alfanumerycznych.")] 
    public string LineNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Opis jest wymagany.")]
    [StringLength(500, MinimumLength = 20, ErrorMessage = "Opis musi mieć od 20 do 500 znaków.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Data zdarzenia jest wymagana.")]
    [DataType(DataType.DateTime)]
    [DateNotInFuture(ErrorMessage = "Data zdarzenia nie może być datą z przyszłości.")]
    public DateTime OccurredAt { get; set; } 
}