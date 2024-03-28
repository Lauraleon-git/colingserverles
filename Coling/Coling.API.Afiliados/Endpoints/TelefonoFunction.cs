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
    public class TelefonoFunction
    {
        private readonly ILogger<TelefonoFunction> _logger;
        private readonly ITelefonoLogic telefonoLogic;
        public TelefonoFunction(ILogger<TelefonoFunction> logger, ITelefonoLogic telefonoLogic)
        {
            _logger = logger;
            this.telefonoLogic = telefonoLogic;
        }

        [Function("ListarTelefono")]
        [OpenApiOperation("Listarspec", "ListarTelefono", Description = "Sirve para listar todas las Telefono")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Telefono>),
         Description = "Mostrara una lista de Telefono")]

        public async Task<HttpResponseData> ListarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarTelefono")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Telefono");
            try
            {
                var listaTelefono = telefonoLogic.ListarTelefonoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTelefono.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarTelefono")]
        [OpenApiOperation("Insertarspec", "InsertarTelefono", Description = "Sirve para Insertar una Telefono")]
        [OpenApiRequestBody("application/json", typeof(Telefono), Description = "Telefono modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Telefono), Description = "Mostrara la Telefono Creada")]

        public async Task<HttpResponseData> InsertarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarTelefono")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Telefono");
            try
            {
                var tel = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un Telefono con todos sus datos");
                bool seGuardo = await telefonoLogic.InsertarTelefono(tel);
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

        [Function("ObtenerTelefonoById")]
        [OpenApiOperation("Obtenerspec", "ObtenerTelefonoById", Description = "Sirve para obtener un Telefono")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Telefono), Description = "Mostrara una Telefono")]

        public async Task<HttpResponseData> ObtenerTelefonoById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerTelefonobyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a una Telefono");
            try
            {
                var tel = telefonoLogic.ObtenerTelefonoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(tel.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ModificarTelefono")]
        [OpenApiOperation("Modificarspec", "ModificarTelefono", Description = "Sirve para Modificar un Telefono")]
        [OpenApiRequestBody("application/json", typeof(Telefono), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Telefono),
         Description = "Mostrara la Telefono modificada")]

        public async Task<HttpResponseData> ModificarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarTelefono/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Telefono");
            try
            {
                var tel = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un telefono con todos sus datos");
                bool seModifico = await telefonoLogic.ModificarTelefono(tel, id);
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
        [Function("EliminarTelefono")]
        [OpenApiOperation("Eliminarspec", "EliminarTelefono", Description = "Sirve para Eliminar un Telefono")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarTelefono/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Telefono");
            try
            {
                bool seElimino = await telefonoLogic.EliminarTelefono(id);
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
