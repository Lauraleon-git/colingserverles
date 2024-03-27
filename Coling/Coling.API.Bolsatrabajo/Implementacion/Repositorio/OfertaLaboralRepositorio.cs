using Coling.API.Bolsatrabajo.Contratos.Repositorio;
using Coling.API.Bolsatrabajo.Modelo;
using Coling.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Implementacion.Repositorio
{
    public class OfertaLaboralRepositorio : IOfertaLaboralRepositorio
    {
        private readonly string? cadenaconexion;
        private readonly IConfiguration configuration;
        private readonly IMongoCollection<OfertaLaboral> collection;

        public OfertaLaboralRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaconexion = configuration.GetSection("cadena").Value;
            var client = new MongoClient(cadenaconexion);
            var database = client.GetDatabase("Cluster0");
            collection = database.GetCollection<OfertaLaboral>("OfertaLaboral");
        }
        public async Task<bool> Eliminar(string id)
        {
            try
            {

                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    var sw = await collection.DeleteOneAsync(Builders<OfertaLaboral>.Filter.Eq("_id", objectId));
                    return sw.DeletedCount == 1;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Insertar(OfertaLaboral ofertaLaboral)
        {
            try
            {
                await collection.InsertOneAsync(ofertaLaboral);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<OfertaLaboral>> ListarOfertas()
        {
            List<OfertaLaboral> lista = new List<OfertaLaboral>();

            lista = await collection.Find(d => true).ToListAsync();
            return lista;
        }

        public async Task<bool> Modificar(OfertaLaboral ofertaLaboral, string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    bool sw = false;
                    OfertaLaboral modificar = await collection.Find(Builders<OfertaLaboral>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
                    if (modificar != null)
                    {
                       

                        await collection.ReplaceOneAsync(Builders<OfertaLaboral>.Filter.Eq("_id", objectId), modificar);
                        sw = true;
                    }
                    return sw;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<OfertaLaboral> ObtenerOfertabyId(string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    var sw = await collection.Find(Builders<OfertaLaboral>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();

                    return sw;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
