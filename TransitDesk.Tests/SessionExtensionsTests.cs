using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TransitDesk.Web.Extensions;
using TransitDesk.Web.Models;
using Xunit;

namespace TransitDesk.Tests;

public class SessionExtensionsTests
{
    private class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _storage = new();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _storage.Keys;

        public void Clear() => _storage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _storage.Remove(key);
        public void Set(string key, byte[] value) => _storage[key] = value;
        public bool TryGetValue(string key, out byte[]? value) => _storage.TryGetValue(key, out value);
    }

    private readonly ISession _session = new TestSession();

    [Fact]
    public void SessionExtensions_SetObjectAndGetObject_SHOULD_SerializeAndDeserializeVehicleListWithoutDataLoss_GIVEN_ComplexVehicleList()
    {
        const string sessionKey = "Watchlist_Test";
        var vehicleList = new List<VehicleViewModel>
        {
            new()
            {
                SideNumber = 1111,
                ModelName = "Solaris Urbino 12",
                DepotName = "NowyPort",
                ProductionYear = 2020,
                Capacity = 90,
                IsLowFloor = true,
                ServiceStatus = ServiceStatus.Active
            },
            new()
            {
                SideNumber = 2072,
                ModelName = "Pesa Swing",
                DepotName = "Wrzeszcz",
                ProductionYear = 2018,
                Capacity = 200,
                IsLowFloor = true,
                ServiceStatus = ServiceStatus.Inactive
            }
        };

        _session.SetObject(sessionKey, vehicleList);
        var retrievedList = _session.GetObject<List<VehicleViewModel>>(sessionKey);

        retrievedList.Should().NotBeNull();
        retrievedList.Should().HaveCount(2);
        retrievedList.Should().BeEquivalentTo(vehicleList);
    }

    [Fact]
    public void SessionExtensions_GetObject_SHOULD_ReturnDefault_GIVEN_NonExistentKey()
    {
        const string nonExistentKey = "NonExistentKey";

        var result = _session.GetObject<List<VehicleViewModel>>(nonExistentKey);

        result.Should().BeNull();
    }

    [Fact]
    public void SessionExtensions_SetObject_SHOULD_OverwriteExistingValue_GIVEN_ExistingKeyWithNewValue()
    {
        const string sessionKey = "Watchlist_Overwrite";
        var initialList = new List<VehicleViewModel>
        {
            new() { SideNumber = 1111, ModelName = "Old Model" }
        };
        var updatedList = new List<VehicleViewModel>
        {
            new() { SideNumber = 2222, ModelName = "New Model" }
        };

        _session.SetObject(sessionKey, initialList);
        _session.SetObject(sessionKey, updatedList);
        var result = _session.GetObject<List<VehicleViewModel>>(sessionKey);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedList);
    }
}
