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
    public class AfiliadoIdiomaFunction
    {
        
            private readonly ILogger<AfiliadoIdiomaFunction> _logger;
            private readonly IAfiliadoIdiomaLogic afiliadoIdiomaLogic;

            public AfiliadoIdiomaFunction(ILogger<AfiliadoIdiomaFunction> logger, IAfiliadoIdiomaLogic afiliadoIdiomaLogic)
            {
                _logger = logger;
                this.afiliadoIdiomaLogic = afiliadoIdiomaLogic;
            }

            [Function("ListarAfiliadosIdiomas")]
            public async Task<HttpResponseData> ListarAfiliadosIdiomas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarafiliadosIdiomas")] HttpRequestData req)
            {
                _logger.LogInformation("Ejecutando Azure Function para Listar AfiliadosIdiomas");
                try
                {
                    var listaAfiliadoIdioma = afiliadoIdiomaLogic.ListarAfiliadoIdiomaTodos();
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(listaAfiliadoIdioma.Result);
                    return respuesta;
                }
                catch (Exception e)
                {
                    var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await error.WriteAsJsonAsync(e.Message);
                    return error;
                }

            }

            [Function("InsertarAfiliadoIdioma")]
            public async Task<HttpResponseData> InsertarAfiliadoIdioma([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarafiliadoIdioma")] HttpRequestData req)
            {
                _logger.LogInformation("Ejecutando Azure Function para Insertar afiliadoIdioma");
                try
                {
                    var af = await req.ReadFromJsonAsync<AfiliadoIdioma>() ?? throw new Exception("Debe ingresar un Afiliado idioma con todos sus datos");
                    bool seGuardo = await afiliadoIdiomaLogic.InsertarAfiliadoIdioma(af);
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

            [Function("ObtenerAfiliadoIdiomaById")]
            public async Task<HttpResponseData> ObtenerAfiliadoIdiomaById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerAfiliadoIdiomabyid/{id}")] HttpRequestData req, int id)
            {
                _logger.LogInformation("Ejecutando Azure Function para Obtener a una AfiliadoIdioma");
                try
                {
                    var idi = afiliadoIdiomaLogic.ObtenerAfiliadoIdiomaById(id);
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
            [Function("ModificarAfiliadoIdioma")]
            public async Task<HttpResponseData> ModificarAfiliadoIdioma([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarafiliadoIdioma/{id}")] HttpRequestData req, int id)
            {
                _logger.LogInformation("Ejecutando Azure Function para Modificar AfiliadoIdioma");
                try
                {
                    var af = await req.ReadFromJsonAsync<AfiliadoIdioma>() ?? throw new Exception("Debe ingresar un AfiliadoIdioma con todos sus datos");
                    bool seModifico = await afiliadoIdiomaLogic.ModificarAfiliadoIdioma(af, id);
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
            [Function("EliminarAfiliadoIdioma")]
            public async Task<HttpResponseData> EliminarAfiliadoIdioma([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarafiliadoIdioma/{id}")] HttpRequestData req, int id)
            {
                _logger.LogInformation("Ejecutando Azure Function para Eliminar AfiliadoIdioma");
                try
                {
                    bool seElimino = await afiliadoIdiomaLogic.EliminarAfiliadoIdioma(id);
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
