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
    public class InstitucionFunction
    {
        private readonly ILogger<InstitucionFunction> _logger;
        private readonly IInstitucionRepositorio repos;



        public InstitucionFunction(ILogger<InstitucionFunction> logger, IInstitucionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarInstitucion")]
        [OpenApiOperation("Insertarspec", "InsertarInstitucion", Description = "Sirve para Insertar una Institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Institucion), Description = "Mostrara la Institucion Creada")]
        public async Task<HttpResponseData> InsertarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresae una persona con todos sus datos");
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

        [Function("ListarInstitucion")]
        [OpenApiOperation("Listarspec", "ListarInstitucion",Description="Sirve para listar todas las instituciones")]
        [OpenApiResponseWithBody(statusCode:HttpStatusCode.OK,contentType:"application/json",bodyType:typeof(List<Institucion>),
            Description="Mostrara una lista de instituciones")]
        public async Task<HttpResponseData> ListarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [Function("ObtenerInstitucionById")]
        [OpenApiOperation("Obtenerspec","ObtenerInstitucionById",Description ="Sirve para obtener una Institucion")]
        [OpenApiParameter(name:"rowkey",In =ParameterLocation.Path,Required =true,Type =typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Institucion), Description ="Mostrara una institucion")]
        public async Task<HttpResponseData> ObtenerInstitucionById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route= "obtenerInstitucionById/{rowkey}")] HttpRequestData req,string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var institucion = repos.Get(rowkey) ;
                respuesta=req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(institucion.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarInstitucion")]
        [OpenApiOperation("Modificarspec","ModificarInstitucion", Description ="Sirve para Modificar una institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion), Description ="Institucion modelo")]
        [OpenApiResponseWithBody(statusCode:HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(Institucion),
            Description ="Mostrara la institucion modificada")]
        public async Task<HttpResponseData> ModificarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "put",Route ="ModificarInstitucion")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresae una persona con todos sus datos");
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
        [Function("EliminarInstitucion")]
        [OpenApiOperation("Eliminarspec","EliminarInstitucion",Description ="Sirve para Eliminar una institucion")]
        [OpenApiParameter(name:"partitionkey",In =ParameterLocation.Path,Required =true,Type =typeof(string))]
        [OpenApiParameter(name:"rowkey",In =ParameterLocation.Path,Required =true,Type =typeof(string))]
        public async Task<HttpResponseData> EliminarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "delete",Route ="EliminarInstituto/{partitionkey}/{rowkey}")] HttpRequestData req,string partitionkey,string rowkey)
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
