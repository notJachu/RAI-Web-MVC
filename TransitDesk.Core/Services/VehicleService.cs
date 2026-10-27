using TransitDesk.Core.Entities;

namespace TransitDesk.Core.Services;

public class VehicleService : IVehiclesService
{
    private readonly List<Vehicle> _vehicles;

    public VehicleService()
    {
        _vehicles = SeedVehicles();
    }

    private List<Vehicle> SeedVehicles()
    {
        return new List<Vehicle>
        {
            new Vehicle(1111, "Road", "MAN", "NowyPort", 2010, 50, false),
            new Vehicle(1112, "Road", "MAN", "NowyPort", 2010, 50, false),
            new Vehicle(1113, "Rail", "PESA", "NowyPort", 2016, 100, true),
        };
    }

    public IEnumerable<Vehicle> GetDepotVehicles(string zaj, string type)
    {
        return _vehicles.Where(v =>
            v.DepotName.Equals(zaj, StringComparison.OrdinalIgnoreCase) &&
            v.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    public Vehicle? GetVehicleBySideNumber(int sideNumber)
    {
        return _vehicles.FirstOrDefault(v => v.SideNumber == sideNumber);
    }

    public int CreateVehicle(Vehicle vehicle)
    {
        _vehicles.Add(vehicle);
        return vehicle.SideNumber;
    }

    public int CreateVehicle(
        int sideNumber,
        string type,
        string modelName,
        string depotName,
        int productionYear,
        int capacity,
        bool isLowFloor)
    {
        var vehicle = new Vehicle(
            sideNumber,
            type,
            modelName,
            depotName,
            productionYear,
            capacity,
            isLowFloor);

        return CreateVehicle(vehicle);
    }
}