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
    public class IdiomaLogic : IIdiomaLogic
    {
        private readonly Contexto contexto;

        public IdiomaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarIdioma(int id)
        {
            bool sw = false;
            Idioma existe = await contexto.Idiomas.FindAsync(id);
            if(existe!=null)
            {
                contexto.Idiomas.Remove(existe);
                await contexto.SaveChangesAsync();
                sw=true;
            }
            return sw;
        }

        public async Task<bool> InsertarIdioma(Idioma idioma)
        {
            bool sw = false;
            contexto.Idiomas.Add(idioma);
            int response = await contexto.SaveChangesAsync();
            if(response==1)
            {
                sw=true;
            }
            return sw;
        }

        public async Task<List<Idioma>> ListarIdiomaTodos()
        {
            var lista = await contexto.Idiomas.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarIdioma(Idioma idioma, int id)
        {
            bool sw = false;
            Idioma edit = await contexto.Idiomas.FindAsync();
            if(edit!=null)
            {
                edit.NombreIdioma = idioma.NombreIdioma;
                edit.Estado=idioma.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<Idioma> ObtenerIdiomaById(int id)
        {
            Idioma idioma = await contexto.Idiomas.FirstOrDefaultAsync(x => x.Id == id);
            return idioma;
        }
    }
}
