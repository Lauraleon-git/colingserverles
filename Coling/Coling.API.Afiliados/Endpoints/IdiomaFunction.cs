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
        [OpenApiOperation("Listarspec", "ListarIdiomas", Description = "Sirve para listar todas las Idiomas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Idioma>),
         Description = "Mostrara una lista de Idiomas")]

        public async Task<HttpResponseData> ListarIdiomas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarIdiomas")] HttpRequestData req)
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
        [OpenApiOperation("Insertarspec", "InsertarIdioma", Description = "Sirve para Insertar una Idioma")]
        [OpenApiRequestBody("application/json", typeof(Idioma), Description = "Idioma modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Idioma), Description = "Mostrara la Idioma Creada")]


        public async Task<HttpResponseData> InsertarIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertaridioma")] HttpRequestData req)
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
        [OpenApiOperation("Obtenerspec", "ObtenerIdiomaById", Description = "Sirve para obtener un Idioma")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Idioma), Description = "Mostrara una Idioma")]

        public async Task<HttpResponseData> ObtenerIdiomaById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerIdiomabyid/{id}")] HttpRequestData req, int id)
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
        [OpenApiOperation("Modificarspec", "ModificarIdioma", Description = "Sirve para Modificar un Idioma")]
        [OpenApiRequestBody("application/json", typeof(Idioma), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Idioma),
         Description = "Mostrara la Idioma modificada")]

        public async Task<HttpResponseData> ModificarIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificaridioma/{id}")] HttpRequestData req, int id)
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
        [OpenApiOperation("Eliminarspec", "EliminarIdioma", Description = "Sirve para Eliminar un Idioma")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminaridioma/{id}")] HttpRequestData req, int id)
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