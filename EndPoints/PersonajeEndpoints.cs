using CatalogoGalactico.Models;
using CatalogoGalactico.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using static CatalogoGalactico.Models.Personaje;
using System.ComponentModel;

namespace CatalogoGalactico.EndPoints
{
    public static class PersonajeEndpoints
    {
        public static void MapPersonajeEndpoints(this WebApplication app)
        {


            app.MapGet("/personajes", (Faccion? faccion, bool? fuerzaSensitivo, PersonajeService service) =>
            {
                if (!faccion.HasValue && !fuerzaSensitivo.HasValue)
                {
                    var personajes = service.MostrarTodos();
                    return Results.Ok(personajes);
                }

                var personajesFiltrados = service.Filtrar(faccion, fuerzaSensitivo);
                return Results.Ok(personajesFiltrados);
            }).WithTags("Personajes")
                   .WithSummary("Consultar personajes")
                    .WithDescription("""
                    Obtiene la coleccion de personajes registrados en el catálogo galactico.

                    Permite aplicar filtros opcionales por faccion y sensibilidad a la Fuerza. 
                    Los filtros pueden utilizarse individualmente o combinarse.

                     FILTROS DISPONIBLES:
                    - faccion: permite seleccionar personajes pertenecientes a Rebelde (0), Imperio(1) o Neutral(2).
                    - fuerzaSensitivo: permite seleccionar personajes sensibles o no sensibles a la Fuerza.

                    EJEMPLOS:
                    - GET /personajes → devuelve todos los personajes.
                    - GET /personajes?faccion=Rebelde → devuelve solamente personajes de la faccion Rebelde.
                    - GET /personajes?fuerzaSensitivo=true → devuelve personajes sensibles a la Fuerza.
                    - GET /personajes?faccion=Rebelde&fuerzaSensitivo=true → combina ambos filtros.

                    La consulta es de solo lectura y no modifica los datos almacenados.
                    """)
                    .Produces<List<Personaje>>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest);






            app.MapGet("/personajes/{id}", (int id, PersonajeService service) =>
            {
                var personaje = service.obtenerPorID(id);

                if(personaje is null)
                {
                    return Results.NotFound(new
                    {
                        error = $"no existe Personaje con Id {id}"
                    });
                }

                return Results.Ok(personaje);
            }
            ).WithTags("Personajes")
                    .WithSummary("Obtener un personaje por ID")
                    .WithDescription("""
                    Obtiene la informacion completa de un personaje mediante su identificador unico.

                    El identificador corresponde al campo Id del personaje y debe ser un valor entero.

                    Si existe un personaje con el identificador proporcionado, la API devuelve sus datos con HTTP 200 OK.

                    Si no existe un personaje con ese identificador, la API devuelve HTTP 404 Not Found indicando que el recurso solicitado no existe.
                    """)
                    .Produces<Personaje>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status404NotFound);



            app.MapPost("/personajes", (CrearPersonajeDTO DTO, PersonajeService service) =>
            {
                if (string.IsNullOrWhiteSpace(DTO.Nombre))
                    return Results.BadRequest("El nombre no puede estar vacío.");

                var personaje = service.Crear(DTO);
                return Results.Created($"/personajes/{personaje.Id}", personaje);
            }
            ).WithTags("Personajes")
            .WithSummary("Crear un nuevo personaje")
            .WithDescription("""
            Registra un nuevo personaje en el catálogo galactico.

            El identificador del personaje es generado automáticamente por la aplicación, por lo que no debe enviarse en la solicitud.

            La solicitud debe proporcionar los datos basicos del personaje:
            - Nombre.
            - Especie.
            - Facción.
            - Afiliación.
            - Estado actual.
            - Indicador de sensibilidad a la Fuerza.

            REGLA DE VALIDACIÓN:
            El nombre del personaje no puede estar vacio ni contener únicamente espacios.

            RESPUESTA:
            Si la solicitud es valida, el personaje se agrega al almacenamiento en memoria y la API devuelve HTTP 201 Created junto con el personaje creado y la ubicación del nuevo recurso.

            Si el nombre está vacio, la API devuelve HTTP 400 Bad Request.
            """)
            .Produces<Personaje>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);







            app.MapPut("/personajes/{id}", (int id, CrearPersonajeDTO DTO, PersonajeService service) =>
            {
                try
                {
                    var personaje = service.Modificar(id, DTO);

                    if (personaje is null)
                    {
                        return Results.NotFound(new
                        {
                            error = $"No existe Personaje con Id {id}"
                        });
                    }

                    return Results.Ok(personaje);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            }).WithTags("Personajes")
            .WithSummary("Modificar un personaje")
            .WithDescription("""
            Actualiza los datos de un personaje existente mediante su identificador.

            El cuerpo de la solicitud debe contener los datos actualizados del personaje. El identificador se obtiene desde la URL y no se incluye dentro del DTO.

            REGLA DE NEGOCIO:
            Un personaje que ya tiene estado Muerto no puede cambiar a Vivo ni a Desconocido. Esta validacion evita que un personaje fallecido sea revivido mediante una modificacion posterior.

            RESPUESTAS:
            - 200 OK: el personaje existe y fue actualizado correctamente.
            - 400 Bad Request: los datos incumplen una regla de negocio.
            - 404 Not Found: no existe un personaje con el identificador indicado.
            """)
            .Produces<Personaje>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);





            app.MapDelete("/personajes/{id}", (int id, PersonajeService service) =>
            {
                try
                {
                    bool eliminado = service.Eliminar(id);

                    if (!eliminado)
                    {
                        return Results.NotFound(new
                        {
                            error = $"No existe Personaje con Id {id}"
                        });
                    }

                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            }).WithTags("Personajes")
            .WithSummary("Eliminar un personaje")
            .WithDescription("""
            Elimina un personaje del catalogo mediante su identificador unico.

            La eliminacion solamente se permite cuando el personaje no tiene relaciones que requieran conservarlo en el sistema.

            REGLAS DE NEGOCIO:
            - No se puede eliminar un personaje que este registrado como participante de algun evento.
            - No se puede eliminar un personaje que tenga una Card asociada.

            Estas restricciones permiten conservar la integridad de las relaciones existentes entre personajes, eventos y Cards.

            RESPUESTAS:
            - 204 No Content: el personaje fue eliminado correctamente.
            - 400 Bad Request: el personaje no puede eliminarse debido a una relacion existente.
            - 404 Not Found: no existe un personaje con el identificador indicado.
            """)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);






            app.MapGet("/personajes/ranking", (string? por, PersonajeService service) =>
            {
                if (por != "poder")
                {
                    return Results.BadRequest(new
                    {
                        error = "Parametro 'por' debe ser 'poder'."
                    });
                }

                var ranking = service.RankingPorPoder();
                return Results.Ok(ranking);

            }).WithTags("Personajes")
            .WithSummary("Obtener ranking de personajes por poder")
            .WithDescription("""
            Obtiene un ranking de personajes ordenado de mayor a menor segun el poder de su Card.

            El parametro 'por' define el criterio utilizado para generar el ranking.

            CRITERIO DISPONIBLE:
            - poder: ordena los personajes de mayor a menor poder.

            El ranking utiliza la relacion entre cada personaje y su Card. Los personajes que no tienen una Card asociada no aparecen en el ranking.

            EJEMPLO:
            GET /personajes/ranking?por=poder

            Si el parametro 'por' no contiene el valor 'poder', la API rechaza la solicitud.
            """)
            .Produces<List<Personaje>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);




            app.MapGet("/personajes/{id}/eventos", (int id, EventoService service) =>
            {
                try
                {
                    var eventos = service.ObtenerEventosPorPersonaje(id);
                    return Results.Ok(eventos);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new
                    {
                        error = ex.Message
                    });
                }
            }).WithTags("Personajes")
            .WithSummary("Obtener los eventos de un personaje")
            .WithDescription("""
            Obtiene todos los eventos en los que aparece registrado un personaje.

            La consulta utiliza el identificador del personaje y busca su presencia dentro de la lista de participantes de cada evento.

            RELACION:
            Un personaje puede participar en varios eventos y un evento puede contener varios personajes. Esta consulta permite recorrer la relacion desde un personaje hacia sus eventos relacionados.

            RESPUESTAS:
            - 200 OK: el personaje existe y se devuelve la lista de eventos relacionados. Si el personaje existe pero no tiene eventos asociados, la lista puede estar vacia.
            - 404 Not Found: no existe un personaje con el identificador indicado.

            La consulta es de solo lectura y no modifica los personajes ni los eventos.
            """)
            .Produces<List<Evento>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        }
    }
}
