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
    public class OfertaLaboralFunction
    {
        private readonly ILogger<OfertaLaboralFunction> _logger;
        private readonly IOfertaLaboralRepositorio repos;

        public OfertaLaboralFunction(ILogger<OfertaLaboralFunction> logger,IOfertaLaboralRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarOfertas")]
        [OpenApiOperation("Listarspec", "ListarOfertas", Description = " Sirve para listar todas las Ofertas Laborales")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OfertaLaboral>), Description = "Mostrar una lista de las Ofertas")]

        public async Task<HttpResponseData> ListarOfertas([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
                var lista = repos.ListarOfertas();
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

        [Function("InsertarOfertas")]
        [OpenApiOperation("InsertarOfertasspec", "InsertarOfertas", Description = " Sirve para listar todas las Ofertas Laborales")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboral), Description = "Oferta Laboral modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OfertaLaboral), Description = "Insertara la Oferta Laboral.")]

        public async Task<HttpResponseData> InsertarSolicitud([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                var registro = await req.ReadFromJsonAsync<OfertaLaboral>() ?? throw new Exception("Debe ingresar una Oferta Laboral con todos sus datos");
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
        [Function("ModificarOferta")]
        [OpenApiOperation("ModificarOfertaspec", "ModificarOferta", Summary = "Modificara una oferta laboral existente")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "se modificar la oferta seleccionada.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "Oferta ID", Description = "El ID de la Ofeta Laboral se modificara.")]

        public async Task<HttpResponseData> ModificarOferta([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarOferta/{id}")] HttpRequestData req,
          string id)
        {
            try
            {
                var ofertaLaboral = await req.ReadFromJsonAsync<OfertaLaboral>() ?? throw new Exception("Debe ingresar una oferta laboral con todos sus datos");
                bool success = await repos.Modificar(ofertaLaboral, id);
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
        [Function("ObtenerOfertaById")]
        [OpenApiOperation("ListarOfertaspec", "ObtenerOferta", Description = " Sirve para obtener una lista por id de la oferta laboral")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OfertaLaboral>), Description = "Listar por ID de la Oferta LAboral")]
        [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Lista de Oferta Laboral por Id")]

        public async Task<HttpResponseData> ObtenerOferta(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerOfertaById/{id}")] HttpRequestData req,
          string id)
        {
            try
            {
                var ofertaLaboral = await repos.ObtenerOfertabyId(id);
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
        [Function("EliminarOfertaLaboral")]
        [OpenApiOperation("Eliminarspec", "EliminarOferta", Description = " sirve para eliminar una oferta laboral")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OfertaLaboral), Description = "Se Eliminar la oferta Laboral")]
        [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Eliminar Oferta Laboral por ID")]

        public async Task<HttpResponseData> EliminarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarOfertaLaboral/{id}")] HttpRequestData req,string id)
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
    }
}
