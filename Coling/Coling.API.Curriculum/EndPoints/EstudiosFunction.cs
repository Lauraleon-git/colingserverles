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
    public class EstudiosFunction
    {
        private readonly ILogger<EstudiosFunction> _logger;
        private readonly IEstudiosRepositorio repos;

        public EstudiosFunction(ILogger<EstudiosFunction> logger , IEstudiosRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarEstudios")]
        public async Task<HttpResponseData> InsertarEstudios([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Estudios>() ?? throw new Exception("Debe ingresar un estudio con todos sus datos");
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

        [Function("ListarEstudios")]
        public async Task<HttpResponseData> ListarEstudios([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        [Function("ObtenerEstudiosById")]
        public async Task<HttpResponseData> ObtenerEstudiosById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerEstudiosById/{rowkey}")] HttpRequestData req, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var estudios = repos.Get(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(estudios.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarEstudios")]
        public async Task<HttpResponseData> ModificarEstudios([HttpTrigger(AuthorizationLevel.Function, "put", Route = "ModificarEstudios")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Estudios>() ?? throw new Exception("Debe ingresae un estudio con todos sus datos");
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
        [Function("EliminarEstudios")]
        public async Task<HttpResponseData> EliminarEstudios([HttpTrigger(AuthorizationLevel.Function, "delete",Route ="EliminarEstudios/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
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
