using Coling.API.Bolsatrabajo.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos.Repositorio
{
    public interface IOfertaLaboralRepositorio
    {
        public Task<bool> Insertar(OfertaLaboral ofertaLaboral);
        public Task<List<OfertaLaboral>> ListarOfertas();
        public Task<bool> Modificar(OfertaLaboral ofertaLaboral, string id);
        public Task<bool> Eliminar(string id);
        public Task<OfertaLaboral> ObtenerOfertabyId(string id);
    }
}
