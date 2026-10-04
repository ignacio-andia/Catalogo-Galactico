using CatalogoGalactico.Models;
using CatalogoGalactico.Services;
using static CatalogoGalactico.Models.CardsPersonaje;

namespace CatalogoGalactico.EndPoints
{
    public static class CardsPersonajesEndpoints
    {
        public static void MapCardPersonajeEndpoints(this WebApplication app)
        {
            // GET /cartas
            app.MapGet("/cartas", (CardPersonajeService service) =>
            {
                var cards = service.MostrarTodo();
                return Results.Ok(cards);
            });


            // GET /cartas/{id}
            app.MapGet("/cartas/{id}", (int id, CardPersonajeService service) =>
            {
                var card = service.ObtenerPorId(id);
                if (card is null)
                {
                    return Results.NotFound(new
                    {
                        error = $"No existe Card con Id {id}"
                    });
                }
                return Results.Ok(card);
            });


            // POST /cartas
            app.MapPost("/cartas", (CrearCardsPersonajeDTO DTO, CardPersonajeService service) =>
            {
                try
                {
                    var card = service.Crear(DTO);

                    return Results.Created(
                        $"/cartas/{card.Id}",
                        card
                    );
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            });


            // PUT /cartas/{id}
            app.MapPut("/cartas/{id}", (int id, CrearCardsPersonajeDTO DTO, CardPersonajeService service) =>
            {
                try
                {
                    var card = service.Modificar(id, DTO);

                    if (card is null)
                    {
                        return Results.NotFound(new
                        {
                            error = $"No existe Card con Id {id}"
                        });
                    }

                    return Results.Ok(card);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            });
        }
    }
}
