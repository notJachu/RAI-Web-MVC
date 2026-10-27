namespace TransitDesk.Core.Entities;

public class Incident : IIncident
{
    public int Id { get; set; }
    public int VehicleSideNumber { get; set; }
    public string LineNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string? AttachmentPath { get; set; }
    public bool IsResolved { get; set; }

    public Incident()
    {
    }

    public Incident(int vehicleSideNumber, string lineNumber, string description, DateTime occurredAt, string? attachmentPath = null, bool isResolved = false)
    {
        Id = Random.Shared.Next();
        VehicleSideNumber = vehicleSideNumber;
        LineNumber = lineNumber;
        Description = description;
        OccurredAt = occurredAt;
        AttachmentPath = attachmentPath;
        IsResolved = isResolved;
    }

    public Incident(int id, int vehicleSideNumber, string lineNumber, string description, DateTime occurredAt, string? attachmentPath, bool isResolved)
    {
        Id = id;
        VehicleSideNumber = vehicleSideNumber;
        LineNumber = lineNumber;
        Description = description;
        OccurredAt = occurredAt;
        AttachmentPath = attachmentPath;
        IsResolved = isResolved;
    }
}
