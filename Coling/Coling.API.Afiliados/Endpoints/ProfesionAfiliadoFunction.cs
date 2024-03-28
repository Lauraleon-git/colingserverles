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
    public class ProfesionAfiliadoFunction
    {
        private readonly ILogger<ProfesionAfiliadoFunction> _logger;
        private readonly IProfesionAfiliadoLogic profesionAfiliadoLogic;

        public ProfesionAfiliadoFunction(ILogger<ProfesionAfiliadoFunction> logger, IProfesionAfiliadoLogic profesionAfiliadoLogic)
        {
            _logger = logger;
            this.profesionAfiliadoLogic = profesionAfiliadoLogic;
        }

        [Function("ListarProfesionAfiliado")]
        public async Task<HttpResponseData> ListarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarProfesionAfiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar ProfesionAfiliado");
            try
            {
                var listaIdioma = profesionAfiliadoLogic.ListarProfesionAfiliadoTodos();
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

        [Function("InsertarProfesionAfiliado")]
        public async Task<HttpResponseData> InsertarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarProfesionAfiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar ProfesionAfiliado");
            try
            {
                var idi = await req.ReadFromJsonAsync<ProfesionAfiliado>() ?? throw new Exception("Debe ingresar una ProfesionAfiliado con todos sus datos");
                bool seGuardo = await profesionAfiliadoLogic.InsertarProfesionAfiliado(idi);
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

        [Function("ObtenerProfesionAfiliadoById")]
        public async Task<HttpResponseData> ObtenerProfesionAfiliadoById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerProfesionAfiliadobyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a una ProfesionAfiliado");
            try
            {
                var idi = profesionAfiliadoLogic.ObtenerProfesionAfiliadoById(id);
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
        [Function("ModificarProfesionAfiliado")]
        public async Task<HttpResponseData> ModificarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProfesionAfiliado/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar ProfesionAfiliado");
            try
            {
                var idi = await req.ReadFromJsonAsync<ProfesionAfiliado>() ?? throw new Exception("Debe ingresar un ProfesionAfiliado con todos sus datos");
                bool seModifico = await profesionAfiliadoLogic.ModificarProfesionAfiliado(idi, id);
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
        [Function("EliminarProfesionAfiliado")]
        public async Task<HttpResponseData> EliminarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarProfesionAfiliado/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar ProfesionAfiliado");
            try
            {
                bool seElimino = await profesionAfiliadoLogic.EliminarProfesionAfiliado(id);
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