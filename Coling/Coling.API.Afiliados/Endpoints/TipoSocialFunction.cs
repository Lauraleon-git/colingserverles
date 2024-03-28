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
    public class TipoSocialFunction
    {
        private readonly ILogger<TipoSocialFunction> _logger;
        private readonly ITipoSocialLogic tipoSocialLogic;

        public TipoSocialFunction(ILogger<TipoSocialFunction> logger,ITipoSocialLogic tipoSocialLogic)
        {
            _logger = logger;
            this.tipoSocialLogic = tipoSocialLogic;
        }

        [Function("ListarTipoSocial")]
        [OpenApiOperation("Listarspec", "ListarTipoSocial", Description = "Sirve para listar todas las TipoSocial")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<TipoSocial>),
         Description = "Mostrara una lista de TipoSocial")]

        public async Task<HttpResponseData> ListarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarTipoSocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar TipoSocial");
            try
            {
                var listaTipo = tipoSocialLogic.ListarTipoSocialTodos();
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

        [Function("InsertarTipoSocial")]
        [OpenApiOperation("Insertarspec", "InsertarTipoSocial", Description = "Sirve para Insertar una TipoSocial")]
        [OpenApiRequestBody("application/json", typeof(TipoSocial), Description = "TipoSocial modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TipoSocial), Description = "Mostrara la TipoSocial Creada")]


        public async Task<HttpResponseData> InsertarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarTipoSocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona");
            try
            {
                var tipo= await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un TipoSocial con todos sus datos");
                bool seGuardo = await tipoSocialLogic.InsertarTipoSocial(tipo);
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

        [Function("ObtenerTipoSocialById")]
        [OpenApiOperation("Obtenerspec", "ObtenerTipoSocialById", Description = "Sirve para obtener un TipoSocial")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TipoSocial), Description = "Mostrara una TipoSocial")]

        public async Task<HttpResponseData> ObtenerTipoSocialById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerTipoSocialbyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a un TipoSocial");
            try
            {
                var idi = tipoSocialLogic.ObtenerTipoSocialById(id);
                var respuesta =req.CreateResponse(HttpStatusCode.OK);
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
        [Function("ModificarTipoSocial")]
        [OpenApiOperation("Modificarspec", "ModificarTipoSocial", Description = "Sirve para Modificar un TipoSocial")]
        [OpenApiRequestBody("application/json", typeof(TipoSocial), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TipoSocial),
          Description = "Mostrara la TipoSocial modificada")]

        public async Task<HttpResponseData> ModificarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar TipoSocial");
            try
            {
                var idi = await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un TipoSocial con todos sus datos");
                bool seModifico = await tipoSocialLogic.ModificarTipoSocial(idi, id);
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
        [Function("EliminarTipoSocial")]
        [OpenApiOperation("Eliminarspec", "EliminarTipoSocial", Description = "Sirve para Eliminar un TipoSocial")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Idioma");
            try
            {
                bool seElimino = await tipoSocialLogic.EliminarTipoSocial(id);
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
