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
            [OpenApiOperation("Listarspec", "ListarAfiliadosIdiomas", Description = "Sirve para listar todas las AfiliadosIdiomas")]
            [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<AfiliadoIdioma>),
             Description = "Mostrara una lista de AfiliadosIdiomas")]

             public async Task<HttpResponseData> ListarAfiliadosIdiomas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarafiliadosIdiomas")] HttpRequestData req)
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

            [OpenApiOperation("Insertarspec", "InsertarAfiliadoIdioma", Description = "Sirve para Insertar una AfiliadoIdioma")]
            [OpenApiRequestBody("application/json", typeof(AfiliadoIdioma), Description = "AfiliadoIdioma modelo")]
            [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AfiliadoIdioma), Description = "Mostrara la AfiliadoIdioma Creada")]
            public async Task<HttpResponseData> InsertarAfiliadoIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarafiliadoIdioma")] HttpRequestData req)
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
            [OpenApiOperation("Obtenerspec", "ObtenerAfiliadoIdiomaById", Description = "Sirve para obtener un AfiliadoIdioma")]
            [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
            [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AfiliadoIdioma), Description = "Mostrara una AfiliadoIdioma")]
             public async Task<HttpResponseData> ObtenerAfiliadoIdiomaById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerAfiliadoIdiomabyid/{id}")] HttpRequestData req, int id)
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


            [OpenApiOperation("Modificarspec", "ModificarAfiliadoIdioma", Description = "Sirve para Modificar un AfiliadoIdioma")]
            [OpenApiRequestBody("application/json", typeof(AfiliadoIdioma), Description = "Institucion modelo")]
            [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AfiliadoIdioma),
          Description = "Mostrara la AfiliadoIdioma modificada")]
            public async Task<HttpResponseData> ModificarAfiliadoIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarafiliadoIdioma/{id}")] HttpRequestData req, int id)
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

            [OpenApiOperation("Eliminarspec", "EliminarAfiliadoIdioma", Description = "Sirve para Eliminar un AfiliadoIdioma")]
            [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
            public async Task<HttpResponseData> EliminarAfiliadoIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarafiliadoIdioma/{id}")] HttpRequestData req, int id)
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
