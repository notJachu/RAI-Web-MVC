using Microsoft.AspNetCore.Mvc;
using TransitDesk.Core.Entities;
using TransitDesk.Core.Services;
using TransitDesk.Web.Extensions;
using TransitDesk.Web.Models;

namespace TransitDesk.Web.Controllers;

public class VehiclesController(IVehiclesService vehiclesService) : Controller
{
    private const string WatchlistSessionKey = "Watchlist";

    [HttpGet("zajezdnia/{zaj}/{type?}")]
    public async Task<IActionResult> GetVehicleList(string zaj, string? type)
    {
        ViewBag.DepotName = zaj;
        ViewBag.TractionType = type;

        var vehicles = string.IsNullOrEmpty(type)
            ? vehiclesService.GetDepotVehicles(zaj, "Road").Concat(vehiclesService.GetDepotVehicles(zaj, "Rail"))
            : vehiclesService.GetDepotVehicles(zaj, type);

        var viewModels = vehicles.Select(MapToViewModel).ToList();

        return View(viewModels);
    }

    [HttpGet("pojazd/{number:fleetSideNo}")]
    public async Task<IActionResult> GetVehicle(int number)
    {
        var vehicle = vehiclesService.GetVehicleBySideNumber(number);
        if (vehicle == null)
        {
            return NotFound();
        }

        var viewModel = MapToViewModel(vehicle);
        return View(viewModel);
    }
    
    [HttpGet("pojazd/create")]
    public async Task<IActionResult> GetVehicleCreatePage()
    {
        return View("Create");
    }

    [HttpPost("pojazd/create")]
    [AuditLog]
    [ValidateAntiForgeryToken]
    public IActionResult Create(VehicleViewModel model)
    {
        if (model.SideNumber < 1000 || model.SideNumber > 9999)
        {
            ModelState.AddModelError(nameof(model.SideNumber), "Numer boczny musi być wartością z zakresu 1000–9999.");
        }

        if (string.IsNullOrWhiteSpace(model.ModelName))
        {
            ModelState.AddModelError(nameof(model.ModelName), "Nazwa modelu pojazdu jest wymagana.");
        }

        if (vehiclesService.GetVehicleBySideNumber(model.SideNumber) != null)
        {
            ModelState.AddModelError(nameof(model.SideNumber), "Pojazd o podanym numerze bocznym już istnieje w ewidencji.");
        }

        if (!ModelState.IsValid)
        {
            return View("Create", model);
        }

        var depot = string.IsNullOrWhiteSpace(model.DepotName) ? "NowyPort" : model.DepotName;
        var type = model.IsLowFloor ? "Rail" : "Road";
        var productionYear = model.ProductionYear > 0 ? model.ProductionYear : DateTime.Now.Year;
        var capacity = model.Capacity > 0 ? model.Capacity : 50;

        var vehicle = new Vehicle(
            model.SideNumber,
            type,
            model.ModelName,
            depot,
            productionYear,
            capacity,
            model.IsLowFloor);

        vehiclesService.CreateVehicle(vehicle);

        TempData["SuccessMessage"] = $"Pojazd #{vehicle.SideNumber} został pomyślnie zarejestrowany.";
        return RedirectToAction(nameof(GetVehicle), new { number = vehicle.SideNumber });
    }

    [AcceptVerbs("GET", "POST")]
    [Route("pojazd/obserwuj/{sideNumber}")]
    public IActionResult AddVehicleToWatchlist(int sideNumber, string? returnUrl = null)
    {
        var watchlist = HttpContext.Session.GetObject<List<int>>(WatchlistSessionKey) ?? new List<int>();
        if (!watchlist.Contains(sideNumber))
        {
            watchlist.Add(sideNumber);
            HttpContext.Session.SetObject(WatchlistSessionKey, watchlist);
            TempData["SuccessMessage"] = $"Dodano pojazd #{sideNumber} do listy monitorowanych.";
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(GetVehicleList), new { zaj = "NowyPort" });
    }

    [AcceptVerbs("GET", "POST")]
    [Route("pojazd/przestan-obserwowac/{sideNumber}")]
    public IActionResult RemoveVehicleFromWatchlist(int sideNumber, string? returnUrl = null)
    {
        var watchlist = HttpContext.Session.GetObject<List<int>>(WatchlistSessionKey) ?? new List<int>();
        if (watchlist.Contains(sideNumber))
        {
            watchlist.Remove(sideNumber);
            HttpContext.Session.SetObject(WatchlistSessionKey, watchlist);
            TempData["SuccessMessage"] = $"Usunięto pojazd #{sideNumber} z listy monitorowanych.";
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(GetVehicleList), new { zaj = "NowyPort" });
    }

    [HttpGet("obserwowane")]
    public IActionResult Watchlist()
    {
        var watchlist = HttpContext.Session.GetObject<List<int>>(WatchlistSessionKey) ?? new List<int>();
        var vehicles = watchlist
            .Select(vehiclesService.GetVehicleBySideNumber)
            .Where(v => v != null)
            .Select(v => MapToViewModel(v!))
            .ToList();

        return View(vehicles);
    }

    private static VehicleViewModel MapToViewModel(Vehicle vehicle)
    {
        return new VehicleViewModel
        {
            SideNumber = vehicle.SideNumber,
            ModelName = vehicle.ModelName,
            DepotName = vehicle.DepotName,
            ProductionYear = vehicle.ProductionYear,
            Capacity = vehicle.Capacity,
            IsLowFloor = vehicle.IsLowFloor,
            ServiceStatus = vehicle.IsInService ? ServiceStatus.Active : ServiceStatus.Inactive
        };
    }
}
