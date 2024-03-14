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
    public class ProfesionAfiliadoLogic : IProfesionAfiliadoLogic
    {
        private readonly Contexto contexto;

        public ProfesionAfiliadoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarProfesionAfiliado(int id)
        {
            bool sw = false;
            ProfesionAfiliado existe = await contexto.ProfesionAfiliados.FindAsync(id);
            if (existe != null)
            {
                contexto.ProfesionAfiliados.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarProfesionAfiliado(ProfesionAfiliado profesionAfiliado)
        {
            bool sw = false;
            contexto.ProfesionAfiliados.Add(profesionAfiliado);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<ProfesionAfiliado>> ListarProfesionAfiliadoTodos()
        {
            var lista = await contexto.ProfesionAfiliados.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarProfesionAfiliado(ProfesionAfiliado profesionAfiliado, int id)
        {
            bool sw = false;
            ProfesionAfiliado edit = await contexto.ProfesionAfiliados.FindAsync();
            if (edit != null)
            {
                edit.IdAfiliado = profesionAfiliado.IdAfiliado;
                edit.IdProfesion = profesionAfiliado.IdProfesion;
                edit.FechaAsignacion=profesionAfiliado.FechaAsignacion;
                edit.NroSelloSib=profesionAfiliado.NroSelloSib;
                edit.Estado = profesionAfiliado.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<ProfesionAfiliado> ObtenerProfesionAfiliadoById(int id)
        {
            ProfesionAfiliado pr= await contexto.ProfesionAfiliados.FirstOrDefaultAsync(x => x.Id == id);
            return pr;
        }
    }
}
