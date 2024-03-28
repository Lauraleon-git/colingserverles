using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
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
    public class DireccionFunction
    {
        private readonly ILogger<DireccionFunction> _logger;
        private readonly IDireccionLogic direccionLogic;

        public DireccionFunction(ILogger<DireccionFunction> logger,IDireccionLogic direccionLogic)
        {
            _logger = logger;
            this.direccionLogic = direccionLogic;
        }

        [Function("ListarDirecciones")]
        [OpenApiOperation("Listarspec", "ListarDirecciones", Description = "Sirve para listar todas las Direcciones")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Direccion>),
        Description = "Mostrara una lista de Direcciones")]
        public async Task<HttpResponseData> ListarDirecciones([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listaridirecciones")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar direcciones");
            try
            {
                var listaDirecciones = direccionLogic.ListarDireccionTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaDirecciones.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarDireccion")]
        [OpenApiOperation("Insertarspec", "InsertarDireccion", Description = "Sirve para Insertar una Direccion")]
        [OpenApiRequestBody("application/json", typeof(Direccion), Description = "Direccion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Direccion), Description = "Mostrara la Direccion Creada")]

        public async Task<HttpResponseData> InsertarDireccion([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarDireccion")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona");
            try
            {
                var idi = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una direccion con todos sus datos");
                bool seGuardo = await direccionLogic.InsertarDireccion(idi);
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

        [Function("ObtenerDireccionById")]
        [OpenApiOperation("Obtenerspec", "ObtenerDireccionById", Description = "Sirve para obtener un Direccion")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Direccion), Description = "Mostrara una Direccion")]

        public async Task<HttpResponseData> ObtenerDireccionById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerDireccionbyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a una Direccion");
            try
            {
                var idi = direccionLogic.ObtenerDireccionById(id);
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
        [Function("ModificarDireccion")]
        [OpenApiOperation("Modificarspec", "ModificarDireccion", Description = "Sirve para Modificar un Direccion")]
        [OpenApiRequestBody("application/json", typeof(Direccion), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Direccion),
          Description = "Mostrara la Direccion modificada")]
        public async Task<HttpResponseData> ModificarDireccion([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificardireccion/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Direccion");
            try
            {
                var idi = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una Direccion con todos sus datos");
                bool seModifico = await direccionLogic.ModificarDireccion(idi, id);
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
        [Function("EliminarDirection")]
        [OpenApiOperation("Eliminarspec", "EliminarDireccion", Description = "Sirve para Eliminar un Direccion")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarDirection([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminardirection/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar direction");
            try
            {
                bool seElimino = await direccionLogic.EliminarDireccion(id);
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
