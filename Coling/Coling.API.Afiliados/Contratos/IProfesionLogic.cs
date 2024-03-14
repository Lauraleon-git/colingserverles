using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IProfesionLogic
    {
        public Task<bool> InsertarProfesion(Profesion profesion);
        public Task<bool> ModificarProfesion(Profesion profesion, int id);
        public Task<bool> EliminarProfesion(int id);
        public Task<List<Profesion>> ListarProfesionTodos();
        public Task<Profesion> ObtenerProfesionById(int id);
    }
}
