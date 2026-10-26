using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuditLogAttribute : ActionFilterAttribute
{
    private const string StopwatchItemKey = "AuditLog_Stopwatch";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Start stopera i zapis do kontekstu bieżącego żądania
        var stopwatch = Stopwatch.StartNew();
        context.HttpContext.Items[StopwatchItemKey] = stopwatch;

        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();
        
        Debug.WriteLine($"[AUDIT START] IP: {ip} | Kontroler: {controller} | Akcja: {action} | UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Odczyt stopera z kontekstu żądania i zatrzymanie pomiaru
        if (context.HttpContext.Items[StopwatchItemKey] is Stopwatch stopwatch)
        {
            stopwatch.Stop();
            context.HttpContext.Response.Headers.TryAdd("X-Execution-Duration-Ms", stopwatch.ElapsedMilliseconds.ToString());
        }
    }
}