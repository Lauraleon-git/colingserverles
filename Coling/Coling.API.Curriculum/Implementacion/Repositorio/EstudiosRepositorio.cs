using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class EstudiosRepositorio : IEstudiosRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string TablaNombre;
        private readonly IConfiguration configuracion;
        public EstudiosRepositorio(IConfiguration conf)
        {
            configuracion = conf;
            cadenaConexion = configuracion.GetSection("cadenaconexion").Value;
            TablaNombre = "Estudios";
        }
        public async Task<bool> Create(Estudios estudios)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, TablaNombre);
                await tablaCliente.UpsertEntityAsync(estudios);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Delete(string partition, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, TablaNombre);
                await tablaCliente.DeleteEntityAsync(partition, rowkey);
                return true;

            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public async Task<Estudios> Get(string rowkey)
        {

            var tablaCliente = new TableClient(cadenaConexion,TablaNombre);
            var experiencia = await tablaCliente.GetEntityAsync<Estudios>("Estudios", rowkey);
            return experiencia.Value;
        }

        public async Task<List<Estudios>> GetAll()
        {
            List<Estudios>lista=new List<Estudios>();
            var tablaCliente = new TableClient(cadenaConexion, TablaNombre);
            var filtro = $"PartitionKey eq 'Estudios'";
            await foreach (Estudios estudios in tablaCliente.QueryAsync<Estudios>(filter:filtro))
            {
                lista.Add(estudios);
            }
            return lista;

        }

        public async Task<bool> Update(Estudios estudios)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, TablaNombre);
                await tablaCliente.UpdateEntityAsync(estudios, estudios.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
