using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> InsertarExperienciaLaboral([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
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
        public async Task<HttpResponseData> ListarExperiencia([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        public async Task<HttpResponseData> ObtenerExperienciaById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerExperienciaById/{rowkey}")] HttpRequestData req, string rowkey)
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
        public async Task<HttpResponseData> ModificarExperiencia([HttpTrigger(AuthorizationLevel.Function, "put", Route = "ModificarExperiencia")] HttpRequestData req)
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
        public async Task<HttpResponseData> EliminarExperiencia([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "EliminarExperiencia/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
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
