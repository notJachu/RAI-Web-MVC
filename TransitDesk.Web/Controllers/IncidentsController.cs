using Microsoft.AspNetCore.Mvc;

namespace TransitDesk.Web.Controllers;

public class IncidentsController : Controller
{

    public async Task<IActionResult> OnPostCreateAsync()
    {
        return RedirectToPage("Home");
    }
}