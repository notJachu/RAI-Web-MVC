using Microsoft.AspNetCore.Mvc;
using TransitDesk.Core.Entities;
using TransitDesk.Core.Services;
using TransitDesk.Web.Models;

namespace TransitDesk.Web.Controllers;

public class IncidentsController(IIncidentsService incidentsService, IVehiclesService vehiclesService) : Controller
{
    [HttpPost]
    [AuditLog]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IncidentViewModel model, IFormFile? attachmentFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (attachmentFile != null && attachmentFile.Length > 0)
        {
            const long maxFileSize = 5 * 1024 * 1024;
            if (attachmentFile.Length > maxFileSize)
            {
                ModelState.AddModelError("AttachmentFile", "Rozmiar załącznika nie może przekraczać 5 MB.");
                return View(model);
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var extension = Path.GetExtension(attachmentFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("AttachmentFile", "Niedozwolony typ pliku. Dozwolone formaty: .jpg, .jpeg, .png, .pdf.");
                return View(model);
            }

            var secureFileName = $"{Guid.NewGuid()}{extension}";

            var storageDir = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Uploads");
            Directory.CreateDirectory(storageDir);

            var targetPath = Path.Combine(storageDir, secureFileName);

            using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await attachmentFile.CopyToAsync(fileStream);
            }

            model.AttachmentPath = targetPath;
        }

        var incident = new Incident(
            model.VehicleSideNumber,
            model.LineNumber,
            model.Description,
            model.OccurredAt,
            model.AttachmentPath,
            isResolved: false);

        incidentsService.CreateIncident(incident);

        TempData["SuccessMessage"] = $"Zgłoszenie incydentu dla pojazdu #{model.VehicleSideNumber} zostało pomyślnie zarejestrowane.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Index()
    {
        var incidents = incidentsService.GetAllIncidents();
        var viewModels = incidents.Select(MapToViewModel).ToList();
        return View(viewModels);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyVehicleAvailable(int vehicleSideNumber)
    {
        var vehicle = vehiclesService.GetVehicleBySideNumber(vehicleSideNumber);
        
        if (vehicle != null)
        {
            return Json(true);
        }
        return Json($"Pojazd #{vehicleSideNumber} nie figuruje w czynnym rejestrze floty.");
    }

    private static IncidentViewModel MapToViewModel(Incident incident)
    {
        return new IncidentViewModel
        {
            Id = incident.Id,
            VehicleSideNumber = incident.VehicleSideNumber,
            LineNumber = incident.LineNumber,
            Description = incident.Description,
            OccurredAt = incident.OccurredAt,
            AttachmentPath = incident.AttachmentPath,
            IsResolved = incident.IsResolved
        };
    }
}
