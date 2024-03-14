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
    public class PersonaTipoSocialFunction
    {
        private readonly ILogger<PersonaTipoSocialFunction> _logger;
        private readonly IPersonaTipoSocialLogic personatipoSocialLogic;

        public PersonaTipoSocialFunction(ILogger<PersonaTipoSocialFunction> logger, IPersonaTipoSocialLogic personatipoSocialLogic)
        {
            _logger = logger;
            this.personatipoSocialLogic = personatipoSocialLogic;
        }

        [Function("ListarPersonaTipoSocial")]
        public async Task<HttpResponseData> ListarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarPersonaTipoSocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar PersonaTipoSocial");
            try
            {
                var listaTipo = personatipoSocialLogic.ListarPersonaTipoSocialTodos();
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

        [Function("InsertarPersonaTipoSocial")]
        public async Task<HttpResponseData> InsertarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarPersonaTipoSocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona");
            try
            {
                var tipo = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un PersonaTipoSocial con todos sus datos");
                bool seGuardo = await personatipoSocialLogic.InsertarPersonaTipoSocial(tipo);
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

        [Function("ObtenerPersonaTipoSocialById")]
        public async Task<HttpResponseData> ObtenerPersonaTipoSocialById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerPersonaTipoSocialbyid/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a un PersonaTipoSocial");
            try
            {
                var idi = personatipoSocialLogic.ObtenerPersonaTipoSocialById(id);
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
        [Function("ModificarPersonaTipoSocial")]
        public async Task<HttpResponseData> ModificarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarPersonaTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar PersonaTipoSocial");
            try
            {
                var idi = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un PersonaTipoSocial con todos sus datos");
                bool seModifico = await personatipoSocialLogic.ModificarPersonaTipoSocial(idi, id);
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
        [Function("EliminarPersonaTipoSocial")]
        public async Task<HttpResponseData> EliminarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarPersonaTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Idioma");
            try
            {
                bool seElimino = await personatipoSocialLogic.EliminarPersonaTipoSocial(id);
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