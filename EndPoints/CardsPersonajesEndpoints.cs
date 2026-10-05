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
            })
            .WithTags("Cards")
            .WithSummary("Consultar todas las Cards")
            .WithDescription("""
            Obtiene todas las Cards de personajes registradas en el catalogo.

            Cada Card contiene los atributos de juego asociados a un personaje, incluyendo su poder, habilidad especial, arma, nivel de peligrosidad e imagen.

            La consulta es de solo lectura y no modifica los datos almacenados.

            Si existen Cards registradas, la API devuelve la coleccion completa.
            Si no existen Cards, devuelve una coleccion vacia.
            """)
            .Produces<List<CardsPersonaje>>(StatusCodes.Status200OK);






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
            })
            .WithTags("Cards")
            .WithSummary("Obtener una Card por ID")
            .WithDescription("""
            Obtiene una Card especifica mediante su identificador unico.

            El identificador corresponde al campo Id propio de la Card y debe ser un valor entero.

            Si existe una Card con el identificador proporcionado, la API devuelve sus datos con HTTP 200 OK.

            Si no existe una Card con ese identificador, la API devuelve HTTP 404 Not Found.
            """)
            .Produces<CardsPersonaje>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);






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
            })
            .WithTags("Cards")
            .WithSummary("Crear una nueva Card")
            .WithDescription("""
            Registra una nueva Card asociada a un personaje existente.

            El identificador de la Card es generado automaticamente por la aplicacion y no debe enviarse en la solicitud.

            REGLAS DE NEGOCIO:
            - El PersonajeId indicado debe corresponder a un personaje existente.
            - Un personaje solamente puede tener una Card asociada.

            Si alguna de estas reglas no se cumple, la API rechaza la solicitud con HTTP 400 Bad Request.

            Si la solicitud es valida, la Card se almacena y la API devuelve HTTP 201 Created junto con la Card creada.
            """)
            .Produces<CardsPersonaje>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);





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
            })
            .WithTags("Cards")
            .WithSummary("Modificar una Card")
            .WithDescription("""
            Actualiza los datos de una Card existente mediante su identificador.

            El identificador de la Card se obtiene desde la URL. Los demas datos se proporcionan mediante el cuerpo de la solicitud.

            REGLAS DE VALIDACION:
            - El PersonajeId indicado debe corresponder a un personaje existente.
            - El personaje indicado no puede tener otra Card diferente a la que se esta modificando.

            RESPUESTAS:
            - 200 OK: la Card existe y fue modificada correctamente.
            - 400 Bad Request: el personaje indicado no existe o ya tiene otra Card asociada.
            - 404 Not Found: no existe una Card con el identificador indicado.
            """)
            .Produces<CardsPersonaje>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
