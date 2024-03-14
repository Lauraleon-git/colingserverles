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
    public class AfiliadoFunction
    {
        private readonly ILogger<AfiliadoFunction> _logger;
        private readonly IAfiliadoLogic afiliadoLogic;

        public AfiliadoFunction(ILogger<AfiliadoFunction> logger, IAfiliadoLogic afiliadoLogic)
        {
            _logger = logger;
            this.afiliadoLogic = afiliadoLogic;
        }

        [Function("ListarAfiliados")]
        public async Task<HttpResponseData> ListarAfiliados([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Idiomas");
            try
            {
                var listaAfiliado = afiliadoLogic.ListarAfiliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaAfiliado.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarAfiliado")]
        public async Task<HttpResponseData> InsertarAfiliado([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarafiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar afiliado");
            try
            {
                var af = await req.ReadFromJsonAsync<Afiliado>() ?? throw new Exception("Debe ingresar un idioma con todos sus datos");
                bool seGuardo = await afiliadoLogic.InsertarAfiliado(af);
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

        [Function("ObtenerAfiliadoById")]
        public async Task<HttpResponseData> ObtenerAfiliadoById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerAfiliadobyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a una Afiliado");
            try
            {
                var idi = afiliadoLogic.ObtenerAfiliadoById(id);
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
        [Function("ModificarAfiliado")]
        public async Task<HttpResponseData> ModificarAfiliado([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarafiliado/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Afiliado");
            try
            {
                var af = await req.ReadFromJsonAsync<Afiliado>() ?? throw new Exception("Debe ingresar un Afiliado con todos sus datos");
                bool seModifico = await afiliadoLogic.ModificarAfiliado(af, id);
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
        [Function("EliminarAfiliado")]
        public async Task<HttpResponseData> EliminarAfiliado([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarafiliado/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Afiliado");
            try
            {
                bool seElimino = await afiliadoLogic.EliminarAfiliado(id);
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
