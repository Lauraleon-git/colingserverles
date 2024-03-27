using Coling.API.Bolsatrabajo.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos.Repositorio
{
    public interface ISolicitud
    {
        public Task<bool> Insertar(Solicitud solicitud);
        public Task<List<Solicitud>> getall();
        public Task<bool> UpdateIns(Solicitud solicitud, string id);
        public Task<bool> Eliminar(string id);
        public Task<Solicitud> ObtenerbyId(string id);
    }
}
