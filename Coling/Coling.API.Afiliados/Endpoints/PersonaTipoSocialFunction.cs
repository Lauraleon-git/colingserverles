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
    public class PersonaTipoSocialFunction
    {
        private readonly ILogger<PersonaTipoSocialFunction> _logger;
        private readonly IPersonaTipoSocialLogic personatipoSocialLogic;

        public PersonaTipoSocialFunction(ILogger<PersonaTipoSocialFunction> logger, IPersonaTipoSocialLogic personatipoSocialLogic)
        {
            _logger = logger;
            this.personatipoSocialLogic = personatipoSocialLogic;
        }

        [Function("ListarPersonaTipoSocial")]
        [OpenApiOperation("Listarspec", "ListarPersonaTipoSocial", Description = "Sirve para listar todas las PersonaTipoSocial")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<PersonaTipoSocial>),
          Description = "Mostrara una lista de PersonaTipoSocial")]
        public async Task<HttpResponseData> ListarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarPersonaTipoSocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar PersonaTipoSocial");
            try
            {
                var listaTipo = personatipoSocialLogic.ListarPersonaTipoSocialTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTipo.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarPersonaTipoSocial")]
        [OpenApiOperation("Insertarspec", "InsertarPersonaTipoSocial", Description = "Sirve para Insertar una PersonaTipoSocial")]
        [OpenApiRequestBody("application/json", typeof(PersonaTipoSocial), Description = "PersonaTipoSocial modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PersonaTipoSocial), Description = "Mostrara la PersonaTipoSocial Creada")]
        public async Task<HttpResponseData> InsertarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarPersonaTipoSocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona");
            try
            {
                var tipo = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un PersonaTipoSocial con todos sus datos");
                bool seGuardo = await personatipoSocialLogic.InsertarPersonaTipoSocial(tipo);
                if (seGuardo)
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

        [Function("ObtenerPersonaTipoSocialById")]
        [OpenApiOperation("Obtenerspec", "ObtenerPersonaTipoSocialById", Description = "Sirve para obtener un PersonaTipoSocial")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PersonaTipoSocial), Description = "Mostrara una PersonaTipoSocial")]

        public async Task<HttpResponseData> ObtenerPersonaTipoSocialById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerPersonaTipoSocialbyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a un PersonaTipoSocial");
            try
            {
                var idi = personatipoSocialLogic.ObtenerPersonaTipoSocialById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(idi.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ModificarPersonaTipoSocial")]
        [OpenApiOperation("Modificarspec", "ModificarPersonaTipoSocial", Description = "Sirve para Modificar un PersonaTipoSocial")]
        [OpenApiRequestBody("application/json", typeof(PersonaTipoSocial), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PersonaTipoSocial),
           Description = "Mostrara la PersonaTipoSocial modificada")]
        public async Task<HttpResponseData> ModificarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarPersonaTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar PersonaTipoSocial");
            try
            {
                var idi = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un PersonaTipoSocial con todos sus datos");
                bool seModifico = await personatipoSocialLogic.ModificarPersonaTipoSocial(idi, id);
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
        [Function("EliminarPersonaTipoSocial")]
        [OpenApiOperation("Eliminarspec", "EliminarPersonaTipoSocial", Description = "Sirve para Eliminar un PersonaTipoSocial")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarPersonaTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Idioma");
            try
            {
                bool seElimino = await personatipoSocialLogic.EliminarPersonaTipoSocial(id);
                if (seElimino)
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