using Coling.API.Bolsatrabajo.Contratos.Repositorio;
using Coling.API.Bolsatrabajo.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.API.Bolsatrabajo.Endpoints
{
    public class SolicitudFunction
    {
        private readonly ILogger<SolicitudFunction> _logger;
        private readonly ISolicitudRepositorio repos;

        public SolicitudFunction(ILogger<SolicitudFunction> logger,ISolicitudRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
            
        }
        [Function("ListarSolicitud")]
        [OpenApiOperation("Listarspec", "ListarSolicitud", Description = " Sirve para listar todas las Solicitudes")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Solicitud>), Description = "Mostrar una lista de las Solicitudes")]

        public async Task<HttpResponseData> ListarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            try
            {
                var lista = repos.ListarSolicitudes();
                var respuest = req.CreateResponse(HttpStatusCode.OK);
                await respuest.WriteAsJsonAsync(lista.Result);
                return respuest;
            }
            catch (Exception)
            {
                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("InsertarSolicitud")]
        [OpenApiOperation("InsertarSolicitudspec", "InsertarSolicitud", Description = " Sirve para listar todas las Solicitudes")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "Soicitudes modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Description = "Insertara la Solicitud.")]

        public async Task<HttpResponseData> InsertarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                var registro = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una Solicitud con todos sus datos");
                bool sw = await repos.Insertar(registro);
                if (sw)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }

            }
            catch (Exception)
            {

                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarSolicitud")]
        [OpenApiOperation("ModificarSolicitudspec", "Modifica una Soliciud", Summary = "Modifica una Solicitud existente en el sistema.")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "se modificar la solicitud seleccionada.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la solicitud", Description = "El ID de la Solicitud a modificar.")]

        public async Task<HttpResponseData> ModificarSolicitud( [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarSolicitud/{id}")] HttpRequestData req,string id)
        {
            try
            {
                var solicitud = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una solicitud con todos sus datos");
                bool success = await repos.Modificar(solicitud, id);
                if (success)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("EliminarSolicitud")]
        [OpenApiOperation("Eliminarspec", "EliminarSolicitud", Description = " sirve para eliminar una solicitud")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Description = "Se Eliminar la solicitud")]
        [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Eliminar Solicitud por ID")]

        public async Task<HttpResponseData> EliminarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarSolicitud/{id}")] HttpRequestData req, string id)
        {
            try
            {
                bool success = await repos.Eliminar(id);
                if (success)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ObtenerSolicitud")]
        [OpenApiOperation("ListarSolicitudspec", "ObtenerSolicitud", Description = " Sirve para obtener una lista de Solicitud")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OfertaLaboral>), Description = "Listar porID de la solicitud")]
        [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Lista de solicitud por Id")]

        public async Task<HttpResponseData> ObtenerSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerSolicitud/{id}")] HttpRequestData req, string id)
        {
            try
            {
                var ofertaLaboral = await repos.ObtenerSolicitudbyId(id);
                if (ofertaLaboral != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(ofertaLaboral);
                    return respuesta;
                }
                else
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

    }
}
