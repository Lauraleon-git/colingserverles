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
        [OpenApiOperation("Insertarspec", "InsertarEstudios", Description = "Sirve para insertar un estudio")]
        [OpenApiRequestBody("application/json", typeof(Estudios), Description = "Estudios modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Estudios), Description = "Mostrara el estudio Creada")]
        public async Task<HttpResponseData> InsertarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
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
        [OpenApiOperation("Listarspec", "ListarEstudios", Description = "Sirve para listar todos los estudios")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Estudios>),
            Description = "Mostrara una lista de estudios")]
        public async Task<HttpResponseData> ListarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [OpenApiOperation("Obtenerspec", "ObtenerEstudiosById", Description = "Sirve para obtener una Estudios")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Estudios), Description = "Mostrara un Estudio")]
        public async Task<HttpResponseData> ObtenerEstudiosById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerEstudiosById/{rowkey}")] HttpRequestData req, string rowkey)
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
        [OpenApiOperation("Modificarspec", "ModificarEstudios", Description = "Sirve para Modificar un Estudio")]
        [OpenApiRequestBody("application/json", typeof(Estudios), Description = "Estudios modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Estudios),
            Description = "Mostrara la Estudios modificada")]
        public async Task<HttpResponseData> ModificarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarEstudios")] HttpRequestData req)
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
        [OpenApiOperation("Eliminarspec", "EliminarEstudios", Description = "Sirve para Eliminar un Estudio")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "delete",Route ="EliminarEstudios/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
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
