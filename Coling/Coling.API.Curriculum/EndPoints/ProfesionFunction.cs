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
    public class ProfesionFunction
    {
        private readonly ILogger<ProfesionFunction> _logger;
        private readonly IProfesionRepositorio repos;

        public ProfesionFunction(ILogger<ProfesionFunction> logger,IProfesionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProfesion")]
        [OpenApiOperation("Insertarspec", "InsertarProfesion", Description = "Sirve para Insertar una Profesion")]
        [OpenApiRequestBody("application/json", typeof(Profesion), Description = "Profesion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Profesion), Description = "Mostrara la Profesion Creada")]
        public async Task<HttpResponseData> InsertarProfesion([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresae una profesion con todos sus datos");
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
        [Function("ListarProfesion")]
        [OpenApiOperation("Listarspec", "ListarProfesion", Description = "Sirve para listar todas las profesiones")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Profesion>),
            Description = "Mostrara una lista de profesiones")]
        public async Task<HttpResponseData> ListarProfesion([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [Function("ObtenerProfesionById")]
        [OpenApiOperation("Obtenerspec", "ObtenerProfesionById", Description = "Sirve para obtener una Profesion")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Profesion), Description = "Mostrara una profesion")]

        public async Task<HttpResponseData> ObtenerProfesionById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerProfesionById/{rowkey}")] HttpRequestData req, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var profesion = repos.Get(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(profesion.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ModificarProfesion")]
        [OpenApiOperation("Modificarspec", "ModificarProfesion", Description = "Sirve para Modificar una Profesion")]
        [OpenApiRequestBody("application/json", typeof(Profesion), Description = "Profesion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Profesion),
            Description = "Mostrara la profesion modificada")]
        public async Task<HttpResponseData> ModificarProfesion([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarProfesion")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar todos los datos de la Profesion");
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
        [Function("EliminarProfesion")]
        [OpenApiOperation("Eliminarspec", "EliminarProfesion", Description = "Sirve para Eliminar una Profesion")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarProfesion([HttpTrigger(AuthorizationLevel.Anonymous, "Delete",Route ="EliminarProfesion/{partitionkey}/{rowkey}")] HttpRequestData req,string partitionkey,string rowkey)
        {
            HttpResponseData respuesta;
            try
            {

                bool sw = await repos.Delete(partitionkey,rowkey);
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
