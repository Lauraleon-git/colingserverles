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
    internal class AfiliadoIdiomaLogic : IAfiliadoIdiomaLogic
    {
        private readonly Contexto contexto;

        public AfiliadoIdiomaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarAfiliadoIdioma(int id)
        {
            bool sw = false;
            AfiliadoIdioma existe = await contexto.AfiliadoIdiomas.FindAsync(id);
            if (existe != null)
            {
                contexto.AfiliadoIdiomas.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma)
        {
            bool sw = false;
            contexto.AfiliadoIdiomas.Add(afiliadoIdioma);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<AfiliadoIdioma>> ListarAfiliadoIdiomaTodos()
        {
            var lista = await contexto.AfiliadoIdiomas.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma, int id)
        {
            bool sw = false;
            AfiliadoIdioma edit = await contexto.AfiliadoIdiomas.FindAsync();
            if (edit != null)
            {

                edit.IdAfiliado = afiliadoIdioma.IdAfiliado;
                edit.IdIdioma=afiliadoIdioma.IdIdioma;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<AfiliadoIdioma> ObtenerAfiliadoIdiomaById(int id)
        {
            AfiliadoIdioma AfIdi = await contexto.AfiliadoIdiomas.FirstOrDefaultAsync(x => x.Id == id);
            return AfIdi;
        }
    }
}
