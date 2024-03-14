using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    internal class DireccionLogic : IDireccionLogic
    {
        private readonly Contexto contexto;

        public DireccionLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarDireccion(int id)
        {
            bool sw = false;
            Direccion existe = await contexto.Direcciones.FindAsync(id);
            if (existe != null)
            {
                contexto.Direcciones.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarDireccion(Direccion direccion)
        {
            bool sw = false;
            contexto.Direcciones.Add(direccion);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Direccion>> ListarDireccionTodos()
        {
            var lista = await contexto.Direcciones.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarDireccion(Direccion direccion, int id)
        {
            bool sw = false;
            Direccion edit = await contexto.Direcciones.FindAsync();
            if (edit != null)
            {
                edit.IdPersona = direccion.IdPersona;
                edit.Descripcion = direccion.Descripcion;
                edit.Estado = direccion.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<Direccion> ObtenerDireccionById(int id)
        {
            Direccion direc = await contexto.Direcciones.FirstOrDefaultAsync(x => x.Id == id);
            return direc;
        }
    }
}
