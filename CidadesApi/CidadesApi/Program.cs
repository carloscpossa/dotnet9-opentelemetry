using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using CidadesApi.Repositorios;
using Scalar.AspNetCore;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddScoped<ICidadeRepositorio, CidadeRepositorio>();

builder.Services.AddOpenApi();

var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var otel = builder.Services.AddOpenTelemetry();

var consultasPorEstadosMetrica = new Meter("CidadesApi", "1.0.0");
var contadorConsultasPorEstado = consultasPorEstadosMetrica.CreateCounter<int>("ConsultasPorEstado.count", description: "Contador de consultas por estado");

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics =>
{
    metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CidadesApi"))
        // Metrics provider from OpenTelemetry
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        .AddMeter(consultasPorEstadosMetrica.Name)
        // Metrics provides by ASP.NET Core in .NET
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel");

    if (!string.IsNullOrEmpty(tracingOtlpEndpoint))
    {
        metrics.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
        });    
    }
    else
    {
        metrics.AddConsoleExporter();
    }
    
});

otel.WithTracing(tracing =>
{
    tracing
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CidadesApi"))
        .AddAspNetCoreInstrumentation()        
        .AddSqlClientInstrumentation()
        .AddHttpClientInstrumentation();
    
    if (!string.IsNullOrEmpty(tracingOtlpEndpoint))
    {
        tracing.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
        });    
    }
    else
    {
        tracing.AddConsoleExporter();
    }
});

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.AddConsoleExporter();

    if (!string.IsNullOrEmpty(tracingOtlpEndpoint))
    {
        options.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
        });
    }
    
});

var app = builder.Build();

app.MapScalarApiReference();
app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/cidades/{uf}", async (string uf, ICidadeRepositorio cidadeRepositorio) =>
    {
        var cidades = await cidadeRepositorio.ObterCidadesAsync(uf);
        contadorConsultasPorEstado.Add(1, new KeyValuePair<string, object?>("UF", uf));
        return Results.Ok(cidades);
    })
    .WithName("ObterCidades")
    .WithSummary("Obter Cidades por UF");

app.Run();