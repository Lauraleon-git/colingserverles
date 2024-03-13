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
    public class PersonaFunction
    {
        private readonly ILogger<PersonaFunction> _logger;
        private readonly IPersonaLogic personaLogic;

        public PersonaFunction(ILogger<PersonaFunction> logger,IPersonaLogic personaLogic)
        {
            _logger = logger;
            this.personaLogic = personaLogic;
        }

        [Function("PersonaFunction")]
        public async Task<HttpResponseData> ListarPersonas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "ListarPersonas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecuntado azure function para insertar personas");
            try
            {
                
                var listaPersonas = personaLogic.ListarPersonaTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaPersonas.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarPersona")]
        public async Task<HttpResponseData> InsertarPersona([HttpTrigger(AuthorizationLevel.Function, "post", Route = "InsertarPersona")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecuntado azure function para insertar personas");
            try
            {
                var per=await req.ReadFromJsonAsync<Persona>()?? throw new Exception("Debe ingresar una Persona con todos los datos");
                bool seGuardo = await personaLogic.InsertarPersona(per);
                if (seGuardo)
                {
                    var respuesta=req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
                
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
    }
}
