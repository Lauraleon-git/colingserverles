using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Coling.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class ProfesionRepositorio : IProfesionRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string TablaNombre;
        private readonly IConfiguration configuration;
        public ProfesionRepositorio(IConfiguration conf)
        {
            configuration=conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            TablaNombre = "Profesion";
        }
        public async Task<bool> Create(Profesion profesion)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, TablaNombre);
                await tablaCliente.UpsertEntityAsync(profesion);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

       
        public async Task<bool> Delete(string partitionkey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,TablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Profesion> Get(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion,TablaNombre);
            var experiencia = await tablaCliente.GetEntityAsync<Profesion>("Profesion", rowkey);
            return experiencia.Value;
        }

        public async Task<List<Profesion>> GetAll()
        {
            List<Profesion> lista = new List<Profesion>();
            var tablaCliente = new TableClient(cadenaConexion, TablaNombre);
            var filtro = $"PartitionKey eq 'Profesion'";
            await foreach(Profesion profesion in tablaCliente.QueryAsync<Profesion>(filter:filtro))
            {
                lista.Add(profesion);
            }
            return lista;
        }

        public async Task<bool> Update(Profesion profesion)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,TablaNombre);
                await tablaCliente.UpdateEntityAsync(profesion,profesion.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
