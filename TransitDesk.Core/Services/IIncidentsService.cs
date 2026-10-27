using TransitDesk.Core.Entities;

namespace TransitDesk.Core.Services;

public interface IIncidentsService
{
    public IEnumerable<Incident> GetAllIncidents();
    public Incident? GetIncidentById(int id);
    public IEnumerable<Incident> GetIncidentsByVehicle(int vehicleSideNumber);
    public int CreateIncident(Incident incident);
    public int CreateIncident(
        int vehicleSideNumber,
        string lineNumber,
        string description,
        DateTime occurredAt,
        string? attachmentPath = null,
        bool isResolved = false);
}
