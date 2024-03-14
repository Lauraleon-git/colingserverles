using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> ListarDirecciones([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listaridirecciones")] HttpRequestData req)
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
        public async Task<HttpResponseData> InsertarDireccion([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarDireccion")] HttpRequestData req)
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
        public async Task<HttpResponseData> ObtenerDireccionById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerDireccionbyid/{id}")] HttpRequestData req, int id)
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
        public async Task<HttpResponseData> ModificarDireccion([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificardireccion/{id}")] HttpRequestData req, int id)
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
        public async Task<HttpResponseData> EliminarDirection([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminardirection/{id}")] HttpRequestData req, int id)
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
