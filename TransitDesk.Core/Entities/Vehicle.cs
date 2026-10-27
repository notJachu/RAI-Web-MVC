namespace TransitDesk.Core.Entities;

public class Vehicle : IVehicle
{
    public int Id { get; set; }
    public int SideNumber { get; set; }
    public string Type { get; set; } 
    public string ModelName { get; set; }  
    public string DepotName { get; set; }  
    public int ProductionYear { get; set; }
    public int Capacity { get; set; }
    public bool IsLowFloor { get; set; }
    public bool IsInService { get; set; }

    public Vehicle(int sideNumber, string type, string modelName, string depotName, int productionYear, int capacity, bool isLowFloor)
    {
        Id = Random.Shared.Next();
        SideNumber = sideNumber;
        Type = type;
        ModelName = modelName;
        DepotName = depotName;
        ProductionYear = productionYear;
        Capacity = capacity;
        IsLowFloor = isLowFloor;
        IsInService = false;
    }

    Vehicle(int id, string type, string modelName, string depotName, int productionYear, int capacity,
        bool isLowFloor, bool isInService)
    {
        Id = id;
        Type = type;
        ModelName = modelName;
        DepotName = depotName;
        ProductionYear = productionYear;
        Capacity = capacity;
        IsLowFloor = isLowFloor;
        IsInService = isInService;
    }
}