using System.ComponentModel.DataAnnotations;

namespace TransitDesk.Web.Models;

public class VehicleViewModel
{
    public int SideNumber { get; set; }
    public string ModelName { get; set; } =  string.Empty;
    public string DepotName { get; set; } =  string.Empty;
    public int ProductionYear { get; set; }
    public int Capacity { get; set; }
    public bool IsLowFloor { get; set; }
    public ServiceStatus ServiceStatus { get; set; }
}