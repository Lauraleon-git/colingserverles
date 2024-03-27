using Coling.API.Bolsatrabajo.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos.Repositorio
{
    public interface ISolicitudRepositorio
    {
        public Task<bool> Insertar(Solicitud solicitud);
        public Task<List<Solicitud>> ListarSolicitudes();
        public Task<bool> Modificar(Solicitud solicitud, string id);
        public Task<bool> Eliminar(string id);
        public Task<Solicitud> ObtenerSolicitudbyId(string id);
    }
}
