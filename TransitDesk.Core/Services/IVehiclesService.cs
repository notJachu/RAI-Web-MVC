using TransitDesk.Core.Entities;

namespace TransitDesk.Core.Services;

public interface IVehiclesService
{
    public IEnumerable<Vehicle> GetDepotVehicles(string zaj, string type);
    public Vehicle? GetVehicleBySideNumber(int sideNumber);
    public int CreateVehicle(Vehicle vehicle);
}