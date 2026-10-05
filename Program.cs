using CatalogoGalactico.EndPoints;
using CatalogoGalactico.Models;
using CatalogoGalactico.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddScoped<PersonajeService>();
builder.Services.AddScoped<CardPersonajeService>();
builder.Services.AddScoped<EventoService>();

var app = builder.Build();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Catálogo Galáctico API");
});

app.MapPersonajeEndpoints();
app.MapEventoEndpoints();
app.MapCardPersonajeEndpoints();

app.Run();





