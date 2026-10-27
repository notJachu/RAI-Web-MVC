using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace TransitDesk.Web.Extensions;

public static class SessionExtensions
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetObject<T>(this ISession session, string key)
    {
        var rawJson = session.GetString(key);
        return rawJson == null ? default : JsonSerializer.Deserialize<T>(rawJson);
    }
}
