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
    internal class PersonaTipoSocialLogic : IPersonaTipoSocialLogic
    {
        private readonly Contexto contexto;

        public PersonaTipoSocialLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarPersonaTipoSocial(int id)
        {
            bool sw = false;
            PersonaTipoSocial existe = await contexto.PersonaTipoSociales.FindAsync(id);
            if (existe != null)
            {
                contexto.PersonaTipoSociales.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial)
        {
            bool sw = false;
            contexto.PersonaTipoSociales.Add(personaTipoSocial);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<PersonaTipoSocial>> ListarPersonaTipoSocialTodos()
        {
            var lista = await contexto.PersonaTipoSociales.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial, int id)
        {
            bool sw = false;
            PersonaTipoSocial edit = await contexto.PersonaTipoSociales.FindAsync();
            if (edit != null)
            {
                edit.IdTipoSocial=personaTipoSocial.IdTipoSocial;
                edit.IdPersona=personaTipoSocial.IdPersona;
                edit.Estado = personaTipoSocial.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<PersonaTipoSocial> ObtenerPersonaTipoSocialById(int id)
        {
            PersonaTipoSocial perTipo = await contexto.PersonaTipoSociales.FirstOrDefaultAsync(x => x.Id == id);
            return perTipo;
        }
    }
}
