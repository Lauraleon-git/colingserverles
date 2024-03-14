using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class ProfesionLogic
    {
        private readonly Contexto contexto;
        public ProfesionLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarProfesion(int id)
        {
            bool sw = false;
            Profesion existe = await contexto.Profesiones.FindAsync(id);
            if (existe != null)
            {
                contexto.Profesiones.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarProfesion(Profesion profesion)
        {
            bool sw = false;
            contexto.Profesiones.Add(profesion);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Profesion>> ListarProfesionTodos()
        {
            var lista = await contexto.Profesiones.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarProfesion(Profesion profesion, int id)
        {
            bool sw = false;
            Profesion edit = await contexto.Profesiones.FindAsync();
            if (edit != null)
            {
                edit.IdGrado = profesion.IdGrado;
                edit.NombreProfesion = profesion.NombreProfesion;
                edit.Estado = profesion.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<Profesion> ObtenerProfesionById(int id)
        {
            Profesion pr = await contexto.Profesiones.FirstOrDefaultAsync(x => x.Id == id);
            return pr;
        }
    }
}
