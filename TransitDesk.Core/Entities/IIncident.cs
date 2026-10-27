namespace TransitDesk.Core.Entities;

public interface IIncident
{
    int Id { get; set; }
    int VehicleSideNumber { get; set; }
    string LineNumber { get; set; }
    string Description { get; set; }
    DateTime OccurredAt { get; set; }
    string? AttachmentPath { get; set; }
    bool IsResolved { get; set; }
}
