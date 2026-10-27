using TransitDesk.Core.Entities;

namespace TransitDesk.Core.Services;

public class IncidentsService : IIncidentsService
{
    private readonly List<Incident> _incidents;

    public IncidentsService()
    {
        _incidents = SeedIncidents();
    }

    private List<Incident> SeedIncidents()
    {
        return new List<Incident>
        {
            new Incident(1111, "199", "Przebita Opona", DateTime.Now.AddDays(-2), null, false),
            new Incident(1112, "115", "Wymuszenie pierwszeństwa na skrzyżowaniu", DateTime.Now.AddDays(-1), null, false),
            new Incident(1113, "10", "Samochód ciężarowy wjechał w tramwaj w kierunku Brętowo PKM na ulicy Wyzwolenia", DateTime.Now.AddHours(-3), null, false),
        };
    }

    public IEnumerable<Incident> GetAllIncidents()
    {
        return _incidents;
    }

    public Incident? GetIncidentById(int id)
    {
        return _incidents.FirstOrDefault(i => i.Id == id);
    }

    public IEnumerable<Incident> GetIncidentsByVehicle(int vehicleSideNumber)
    {
        return _incidents.Where(i => i.VehicleSideNumber == vehicleSideNumber);
    }

    public int CreateIncident(Incident incident)
    {
        _incidents.Add(incident);
        return incident.Id;
    }

    public int CreateIncident(
        int vehicleSideNumber,
        string lineNumber,
        string description,
        DateTime occurredAt,
        string? attachmentPath = null,
        bool isResolved = false)
    {
        var incident = new Incident(
            vehicleSideNumber,
            lineNumber,
            description,
            occurredAt,
            attachmentPath,
            isResolved);

        return CreateIncident(incident);
    }
}
