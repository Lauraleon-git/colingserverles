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
    public class TipoSocialLogic : ITipoSocialLogic
    {
        private readonly Contexto contexto;

        public TipoSocialLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarTipoSocial(int id)
        {
            bool sw = false;
            TipoSocial existe = await contexto.TiposSociales.FindAsync(id);
            if (existe != null)
            {
                contexto.TiposSociales.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarTipoSocial(TipoSocial tipoSocial)
        {
            bool sw = false;
            contexto.TiposSociales.Add(tipoSocial);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<TipoSocial>> ListarTipoSocialTodos()
        {
            var lista = await contexto.TiposSociales.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarTipoSocial(TipoSocial tipoSocial, int id)
        {
            bool sw = false;
            TipoSocial edit = await contexto.TiposSociales.FindAsync();
            if (edit != null)
            {
                edit.NombreSocial=tipoSocial.NombreSocial;
                edit.Estado = tipoSocial.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<TipoSocial> ObtenerTipoSocialById(int id)
        {
            TipoSocial tipo = await contexto.TiposSociales.FirstOrDefaultAsync(x => x.Id == id);
            return tipo;
        }
    }
}
