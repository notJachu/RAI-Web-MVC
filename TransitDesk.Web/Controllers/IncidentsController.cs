using Microsoft.AspNetCore.Mvc;
using TransitDesk.Web.Models;

namespace TransitDesk.Web.Controllers;

public class IncidentsController : Controller
{

    [HttpPost]
    public async Task<IActionResult> Create(IncidentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: add service
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyVehicleAvailable(int vehicleSideNumber)
    {
        bool isActive = true; // TODO: ADD VEHICLE SERVICE
        
        if (isActive)
        {
            return Json(new { isActive = true });
        }
        return Json(new { isActive = false });
    }
}