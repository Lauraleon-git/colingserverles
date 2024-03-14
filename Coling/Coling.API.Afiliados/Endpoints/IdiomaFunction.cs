using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Afiliados.Endpoints
{
    public class IdiomaFunction
    {
        private readonly ILogger<IdiomaFunction> _logger;
        private readonly IIdiomaLogic idiomaLogic;

        public IdiomaFunction(ILogger<IdiomaFunction> logger,IIdiomaLogic idiomaLogic)
        {
            _logger = logger;
            this.idiomaLogic = idiomaLogic;
        }

        [Function("ListarIdiomas")]
        public async Task<HttpResponseData> ListarIdiomas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarIdiomas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Idiomas");
            try
            {
                var listaIdioma = idiomaLogic.ListarIdiomaTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaIdioma.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarIdioma")]
        public async Task<HttpResponseData> InsertarIdioma([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertaridioma")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona");
            try
            {
                var idi = await req.ReadFromJsonAsync<Idioma>() ?? throw new Exception("Debe ingresar un idioma con todos sus datos");
                bool seGuardo = await idiomaLogic.InsertarIdioma(idi);
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

        [Function("ObtenerIdiomaById")]
        public async Task<HttpResponseData> ObtenerIdiomaById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerIdiomabyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a una Persona");
            try
            {
                var idi = idiomaLogic.ObtenerIdiomaById(id);
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
        [Function("ModificarIdioma")]
        public async Task<HttpResponseData> ModificarIdioma([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificaridioma/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Idioma");
            try
            {
                var idi = await req.ReadFromJsonAsync<Idioma>() ?? throw new Exception("Debe ingresar un idioma con todos sus datos");
                bool seModifico = await idiomaLogic.ModificarIdioma(idi, id);
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
        [Function("EliminarIdioma")]
        public async Task<HttpResponseData> EliminarIdioma([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminaridioma/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Idioma");
            try
            {
                bool seElimino = await idiomaLogic.EliminarIdioma(id);
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