using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.API.Curriculum.EndPoints
{
    public class ExperienciaLaboralFunction
    {
        private readonly ILogger<ExperienciaLaboralFunction> _logger;
        private readonly IExperienciaLaboralRepositorio repos;

        public ExperienciaLaboralFunction(ILogger<ExperienciaLaboralFunction> logger,IExperienciaLaboralRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarExperienciaLaboral")]
        [OpenApiOperation("Insertarspec", "InsertarExperienciaLaboral", Description = "Sirve para Insertar una ExperienciaLaboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral), Description = "ExperienciaLaboral modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExperienciaLaboral), Description = "Mostrara la Experiencia Laboral Creada")]
        public async Task<HttpResponseData> InsertarExperienciaLaboral([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresae una persona con todos sus datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.Now;
                bool sw = await repos.Create(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;

                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ListarExperiencia")]
        [OpenApiOperation("Listarspec", "ListarExperiencia", Description = "Sirve para listar todas las experiencias laborales")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ExperienciaLaboral>),
            Description = "Mostrara una lista de Experiencias laborales")]
        public async Task<HttpResponseData> ListarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;

            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ObtenerExperienciaById")]
        [OpenApiOperation("Obtenerspec", "ObtenerExperienciaById", Description = "Sirve para obtener una Experiencia")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExperienciaLaboral), Description = "Mostrara una Experiencia Laboral")]

        public async Task<HttpResponseData> ObtenerExperienciaById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerExperienciaById/{rowkey}")] HttpRequestData req, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var exp = repos.Get(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(exp.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
     
        }
        [Function("ModificarExperiencia")]
        [OpenApiOperation("Modificarspec", "ModificarExperiencia", Description = "Sirve para Modificar una Experiencia")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral), Description = "Experiencia modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExperienciaLaboral),
            Description = "Mostrara la experiencia laboral modificada")]
        public async Task<HttpResponseData> ModificarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarExperiencia")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresae una persona con todos sus datos");
                bool sw = await repos.Update(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;

                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("EliminarExperiencia")]
        [OpenApiOperation("Eliminarspec", "EliminarExperencia", Description = "Sirve para Eliminar una Experiencia")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarExperiencia/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                bool sw = await repos.Delete(partitionkey, rowkey);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;

                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}
