using CidadesApi.Repositorios;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICidadeRepositorio, CidadeRepositorio>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapScalarApiReference();
app.MapOpenApi();

// if (app.Environment.IsDevelopment())
// {
//     app.MapScalarApiReference();
//     app.MapOpenApi();
// }

app.UseHttpsRedirection();

app.MapGet("/cidades/{uf}", async (string uf, ICidadeRepositorio cidadeRepositorio) =>
    {
        var cidades = await cidadeRepositorio.ObterCidadesAsync(uf);
        return Results.Ok(cidades);
    })
    .WithName("ObterCidades")
    .WithSummary("Obter Cidades por UF");

app.Run();