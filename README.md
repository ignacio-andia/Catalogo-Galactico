# Catalogo Galactico de Personajes y Eventos


API REST desarrollada en C# con ASP.NET Core Minimal API para gestionar personajes, Cards de personajes y eventos de un catalogo galactico.


El proyecto utiliza almacenamiento en memoria y expone operaciones CRUD, relaciones entre entidades, filtros, ranking, simulacion de eventos y consulta de MVP.


## Tecnologias


- C#

- .NET / ASP.NET Core

- Minimal API

- Swagger / OpenAPI

- Postman

- Git / GitHub


## Objetivo del proyecto


Implementar una API REST capaz de administrar:


- Personajes

- Cards asociadas a personajes

- Eventos

- Relaciones entre personajes y eventos

- Simulacion de eventos

- Estadisticas y ranking


La informacion se almacena en memoria, por lo que puede volver a su estado inicial al reiniciar la aplicacion.


## Modelo de datos


### Personaje


Campos principales:


- Id

- Nombre

- Especie

- Faccion

- Afiliacion

- Estado

- FuerzaSensitiva


Facciones:


- Rebelde

- Imperio

- Neutral


Estados:


- Vivo

- Muerto

- Desconocido


### CardsPersonaje


Cada personaje puede tener una Card asociada.


Campos:


- Id

- PersonajeId

- poder

- HabilidadEspecial

- Arma

- NivelDePeligrosidad

- ImegenURL


La relacion entre Personaje y CardsPersonaje es 1:1.


### Evento


Campos:


- Id

- Nombre

- fecha

- Ubicacion

- Descripcion

- Participantes

- Resultado


Un evento puede tener varios personajes participantes y un personaje puede participar en varios eventos.


La relacion entre Personaje y Evento es N:N.


La relacion se representa mediante la lista Participantes, que almacena los identificadores de los personajes.


## Reglas de negocio


### Personajes


- No se puede modificar el estado de un personaje muerto para revivirlo.

- Un personaje no puede eliminarse si esta registrado en algun evento.

- Un personaje no puede eliminarse si tiene una Card asociada.


### Cards


- La Card solo puede asociarse a un personaje existente.

- Un personaje no puede tener mas de una Card.

- Al modificar una Card tampoco se permite asociarla a un personaje que ya tenga otra Card.


### Eventos


- Todos los participantes deben existir.

- Un personaje muerto no puede participar en un nuevo evento.

- La validacion de participantes se realiza antes de almacenar o modificar el evento.

- Si una validacion falla, el evento no se modifica.


## Simulacion de eventos


La simulacion se ejecuta mediante:


POST /eventos/{id}/simular


Condiciones necesarias:


- El evento debe existir.

- El evento debe tener participantes.

- Debe existir al menos un participante Rebelde y uno Imperial.

- Todos los participantes deben existir.

- Todos los participantes deben tener una Card.

- Los participantes neutrales no intervienen en el combate ni aportan poder.


### Calculo de poder


Para cada faccion:


1. Se suman los valores de poder de las Cards de sus participantes.

2. Se genera un factor aleatorio entre 0.90 y 1.10.

3. Se multiplica el poder base por el factor.

4. La faccion con mayor poder final gana.


Formula:


Poder final = Poder base * Factor aleatorio


### Actualizacion de estados


- Los participantes de la faccion perdedora pasan a estado Muerto.

- Los participantes de la faccion ganadora mantienen su estado.

- Los participantes neutrales no participan y no mueren.

- El resultado se guarda dentro del evento.


La respuesta de la simulacion incluye:


- Id del evento

- Poder base de cada faccion

- Factor aleatorio de cada faccion

- Poder final de cada faccion

- Ganador

- Criterio utilizado


## MVP


La consulta del MVP se realiza mediante:


GET /eventos/{id}/mvp


Reglas:


- El evento debe existir.

- El evento debe haber sido simulado.

- Todos los participantes deben tener Card.

- Los personajes muertos no pueden ser MVP.

- Los participantes neutrales no pueden ser MVP.

- Se determina primero la faccion con mayor cantidad de participantes vivos.

- Dentro de esa faccion se selecciona el participante con mayor poder de Card.


## Ranking


Endpoint:


GET /personajes/ranking


El ranking ordena los personajes de mayor a menor poder utilizando las Cards asociadas.


Los personajes sin Card no aparecen en el ranking.


## Filtros


Endpoint:


GET /personajes


Parametros disponibles:


- faccion

- fuerzaSensitivo


Los filtros pueden combinarse.


Ejemplo:


GET /personajes?faccion=Rebelde&fuerzaSensitivo=true


## Endpoints


### Personajes


GET /personajes

- Obtiene todos los personajes.


GET /personajes/{id}

- Obtiene un personaje por identificador.


POST /personajes

- Crea un personaje.


PUT /personajes/{id}

- Modifica un personaje.


DELETE /personajes/{id}

- Elimina un personaje cuando no existen relaciones que lo impidan.


GET /personajes/ranking

- Obtiene el ranking por poder de Card.


GET /personajes/{id}/eventos

- Obtiene los eventos en los que participa un personaje.


### Cards


GET /cartas

- Obtiene todas las Cards.


GET /cartas/{id}

- Obtiene una Card por identificador.


POST /cartas

- Crea una Card asociada a un personaje.


PUT /cartas/{id}

- Modifica una Card existente.


### Eventos


GET /eventos

- Obtiene todos los eventos.


GET /eventos/{id}

- Obtiene un evento por identificador.


POST /eventos

- Crea un evento.


PUT /eventos/{id}

- Modifica un evento.


POST /eventos/{id}/simular

- Ejecuta la simulacion del evento.


GET /eventos/{id}/mvp

- Obtiene el MVP del evento simulado.


## Codigos HTTP utilizados


### 200 OK


Operacion realizada correctamente.


Se utiliza principalmente en consultas, modificaciones y simulaciones exitosas.


### 201 Created


Recurso creado correctamente.


Se utiliza en:


- POST /personajes

- POST /cartas

- POST /eventos


### 400 Bad Request


La solicitud no cumple una regla de validacion o de negocio.


Ejemplos:


- Participante inexistente.

- Personaje muerto intentando participar en un nuevo evento.

- Personaje sin Card al simular.

- Personaje duplicado en Cards.

- Intento de revivir un personaje muerto.


### 404 Not Found


El recurso solicitado no existe.


Ejemplos:


- Personaje inexistente.

- Card inexistente.

- Evento inexistente.


## Convencion de fechas


La fecha de los eventos se representa mediante un numero entero correspondiente a la cronologia del catalogo.


Ejemplos:


- -10 = 10 BBY

- -5 = 5 BBY

- 0 = Batalla de Yavin

- 3 = 3 ABY

- 10 = 10 ABY


## Estructura del proyecto


La organizacion principal del proyecto separa modelos, almacenamiento, servicios y endpoints.


Models/

- Personaje.cs

- CardsPersonaje.cs

- Evento.cs

- Faccion.cs

- EstadoPersonaje.cs


Data/

- PersonajesStore.cs

- CardsPersonajesStore.cs

- EventosStore.cs


Services/

- PersonajeService.cs

- CardPersonajeService.cs

- EventoService.cs


EndPoints/

- PersonajeEndPoints.cs

- CardPersonajeEndPoints.cs

- EventoEndPoints.cs


Program.cs

- Configuracion de servicios.

- Configuracion de OpenAPI.

- Configuracion de Swagger UI.

- Registro de endpoints.


## Swagger


La API utiliza OpenAPI y Swagger UI para documentar los endpoints, parametros, modelos y codigos de respuesta.


Al ejecutar la aplicacion se puede acceder a:


http://localhost:5257/swagger


La documentacion incluye:


- Resumen de cada endpoint.

- Descripcion de funcionamiento.

- Reglas de negocio.

- Parametros.

- Modelos de respuesta.

- Codigos HTTP.


## Ejecucion


1. Abrir el proyecto en Visual Studio o mediante terminal.

2. Restaurar las dependencias si es necesario.

3. Ejecutar:


dotnet run


4. Abrir Swagger en:


http://localhost:5257/swagger


## Pruebas realizadas


Las operaciones principales fueron verificadas mediante Postman.


Pruebas realizadas:


- GET de personajes.

- GET de personaje por ID.

- GET de personaje inexistente.

- Filtros por faccion.

- Filtros por sensibilidad a la Fuerza.

- Combinacion de filtros.

- Ranking por poder.

- POST de personaje valido.

- Validacion de personaje invalido.

- PUT de personaje.

- DELETE de personaje.

- Bloqueo de eliminacion cuando tiene Card.

- Bloqueo de eliminacion cuando participa en eventos.

- GET de Cards.

- POST de Card valida.

- Bloqueo de Card duplicada.

- Bloqueo de Card con personaje inexistente.

- PUT de Card.

- GET de eventos.

- POST de evento valido.

- Bloqueo de participante inexistente.

- Bloqueo de personaje muerto como nuevo participante.

- PUT de evento.

- Simulacion de eventos.

- Actualizacion automatica de estados despues de una simulacion.

- Consulta del MVP.

- Bloqueo de MVP antes de simular.

- Bloqueo de simulacion cuando un participante no tiene Card.

- Verificacion de que una simulacion invalida no guarda resultado parcial.


## Datos iniciales


El proyecto incluye personajes, Cards y eventos iniciales para facilitar las pruebas.


Entre los personajes iniciales se encuentran:


- Luke Skywalker

- Darth Vader

- Leia Organa

- Yoda

- Miki

- Koyun

- Boba Fett

- Hondo Ohnaka

- Ahsoka Tano

- Mon Mothma


Tambien se incluyen eventos iniciales como:


- Batalla de Yavin

- Batalla de Hoth

- Enfrentamiento en Tatooine

- Incidente en Endor

- Operacion de la Rebelion


## Objetivo academico


El proyecto busca demostrar el uso de:


- ASP.NET Core Minimal API.

- Programacion orientada a objetos.

- Modelado de entidades y relaciones.

- DTOs.

- Servicios para reglas de negocio.

- Endpoints REST.

- Codigos de estado HTTP.

- Consultas LINQ.

- Simulacion con aleatoriedad controlada.

- Actualizacion de estado de entidades relacionadas.

- Documentacion mediante Swagger/OpenAPI.

- Pruebas mediante Postman.

- Control de versiones con Git.


