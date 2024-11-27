using System.Diagnostics.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using CidadesApi.Repositorios;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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
otel.WithMetrics(metrics => metrics
    // Metrics provider from OpenTelemetry
    .AddAspNetCoreInstrumentation()
    .AddMeter(consultasPorEstadosMetrica.Name)
    // Metrics provides by ASP.NET Core in .NET
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    .AddPrometheusExporter());

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

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