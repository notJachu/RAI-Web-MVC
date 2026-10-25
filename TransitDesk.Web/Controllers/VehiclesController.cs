using Microsoft.AspNetCore.Mvc;

namespace TransitDesk.Web.Controllers;

public class VehiclesController : Controller
{
    [HttpGet("zajezdnia/{zaj}/{type?}")]
    public async Task<IActionResult> GetVehicleList(string zaj, string type)
    {
        return View();
    }

    [HttpGet("pojazd/{number:fleetSideNumber}")]
    public async Task<IActionResult> GetVehicle(int number)
    {
        return View();
    }
}