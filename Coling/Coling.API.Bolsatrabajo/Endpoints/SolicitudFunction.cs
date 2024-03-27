using Coling.API.Bolsatrabajo.Contratos.Repositorio;
using Coling.API.Bolsatrabajo.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Bolsatrabajo.Endpoints
{
    public class SolicitudFunction
    {
        private readonly ILogger<SolicitudFunction> _logger;
        private readonly ISolicitud repos;

        public SolicitudFunction(ILogger<SolicitudFunction> logger,ISolicitud repos)
        {
            _logger = logger;
            this.repos = repos;
            
        }

        [Function("InsertarSolicitud")]
        [OpenApiOperation("Insertarspec", "InsertarSolicitud", Description = " Sirve para listar todas las Solicitudes")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "Oferta Laboral modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Description = "Insertara la Oferta Laboral.")]

        public async Task<HttpResponseData> InsertarSolicitud([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                var registro = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una institucion con todos sus datos");
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
    }
}
