using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.API.Afiliados.Endpoints
{
    public class PersonaFunction
    {
        private readonly ILogger<PersonaFunction> _logger;
        private readonly IPersonaLogic personaLogic;

        public PersonaFunction(ILogger<PersonaFunction> logger,IPersonaLogic personaLogic)
        {
            _logger = logger;
            this.personaLogic = personaLogic;
        }

        [Function("ListarPersonas")]
        [OpenApiOperation("Listarspec", "ListarPersonas", Description = "Sirve para listar todas las Personas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Persona>),
         Description = "Mostrara una lista de Personas")]
        public async Task<HttpResponseData> ListarPersonas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarPersonas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecuntado azure function para insertar personas");
            try
            {
                
                var listaPersonas = personaLogic.ListarPersonaTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaPersonas.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarPersona")]
        [OpenApiOperation("Insertarspec", "InsertarPersona", Description = "Sirve para Insertar una Persona")]
        [OpenApiRequestBody("application/json", typeof(Persona), Description = "Persona modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Persona), Description = "Mostrara la Persona Creada")]

        public async Task<HttpResponseData> InsertarPersona([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "InsertarPersona")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecuntado azure function para insertar personas");
            try
            {
                var per = await req.ReadFromJsonAsync<Persona>() ?? throw new Exception("Debe ingresar una Persona con todos los datos");
                bool seGuardo = await personaLogic.InsertarPersona(per);
                if (seGuardo)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        
        [Function("ModificarPersona")]
        [OpenApiOperation("Modificarspec", "ModificarPersona", Description = "Sirve para Modificar un Persona")]
        [OpenApiRequestBody("application/json", typeof(Persona), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Persona),
         Description = "Mostrara la Persona modificada")]

        public async Task<HttpResponseData> ModificarPersona([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarpersona/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Persona");
            try
            {
                var per = await req.ReadFromJsonAsync<Persona>() ?? throw new Exception("Debe ingresar una Persona con todos los datos");
                bool seModifico = await personaLogic.ModificarPersona(per, id);
                if (seModifico)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ObtenerPersonaById")]

        [OpenApiOperation("Obtenerspec", "ObtenerPersonaById", Description = "Sirve para obtener un Persona")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Persona), Description = "Mostrara una Persona")]

        public async Task<HttpResponseData> ObtenerPersonaById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerpersonabyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a una Persona");
            try
            {
                var persona = personaLogic.ObtenerPersonaById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(persona.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("EliminarPersona")]
        [OpenApiOperation("Eliminarspec", "EliminarPersona", Description = "Sirve para Eliminar un Persona")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarPersona([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarPersona/{id}")] HttpRequestData req,int id)
            {
                _logger.LogInformation("Ejecuntado azure function para eliminar personas");
                try
                {
                bool seElimino = await personaLogic.EliminarPersona(id);
                if(seElimino)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
                }
                catch (Exception e)
                {
                    var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await error.WriteAsJsonAsync(e.Message);
                    return error;
                }
            }
        }
}
