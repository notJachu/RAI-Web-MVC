namespace TransitDesk.Core.Entities;

public interface IVehicle
{
    int Id { get; set; }
    int SideNumber { get; set; }
    string Type { get; set; }
    string ModelName {get; set;}
    string DepotName { get; set; }
    int ProductionYear { get; set; }
    int Capacity { get; set; }
    bool IsLowFloor { get; set; }
    bool IsInService { get; set; }
}