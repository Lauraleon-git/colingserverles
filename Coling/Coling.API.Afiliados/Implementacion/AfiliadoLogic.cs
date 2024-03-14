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
    public class AfiliadoLogic : IAfiliadoLogic
    {
        private readonly Contexto contexto;
        public AfiliadoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarAfiliado(int id)
        {
            bool sw = false;
            Afiliado existe = await contexto.Afiliados.FindAsync(id);
            if (existe != null)
            {
                contexto.Afiliados.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }
    

        public async Task<bool> InsertarAfiliado(Afiliado afiliado)
        {
        bool sw = false;
        contexto.Afiliados.Add(afiliado);

        int response = await contexto.SaveChangesAsync();
        if (response == 1)
        {
            sw = true;
        }
        return sw;
    }

        public async Task<List<Afiliado>> ListarAfiliadoTodos()
        {
            var lista = await contexto.Afiliados.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarAfiliado(Afiliado afiliado, int id)
        {
            bool sw = false;
            Afiliado edit = await contexto.Afiliados.FindAsync();
            if (edit != null)
            {
                edit.IdPersona = afiliado.IdPersona;
                edit.FechaAfilacion=afiliado.FechaAfilacion;
                edit.CodigoAfiliado=afiliado.CodigoAfiliado;
                edit.NroTituloProvisional = afiliado.CodigoAfiliado;
                edit.Estado=afiliado.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<Afiliado> ObtenerAfiliadoById(int id)
        {
            Afiliado afiliado = await contexto.Afiliados.FirstOrDefaultAsync(x => x.Id == id);
            return afiliado;
        }
    }
}
