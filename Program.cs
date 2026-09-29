using CatalogoGalactico.EndPoints;
using CatalogoGalactico.Models;
using CatalogoGalactico.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<PersonajeService>();
builder.Services.AddScoped<CardPersonajeService>();
builder.Services.AddScoped<EventoService>();

var app = builder.Build();

app.MapPersonajeEndpoints();
app.MapEventoEndpoints();
app.MapCardPersonajeEndpoints();

app.Run();





