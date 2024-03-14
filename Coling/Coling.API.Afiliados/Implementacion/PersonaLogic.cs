using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class PersonaLogic : IPersonaLogic
    {
        private readonly Contexto contexto;

        public PersonaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarPersona(int id)
        {
            bool sw = false;
            Persona EliminarPer = await contexto.Personas.FindAsync(id);
            if (EliminarPer != null)
            {
                contexto.Personas.Remove(EliminarPer);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }
    

        public async Task<bool> InsertarPersona(Persona persona)
        {
            bool sw = false;
            contexto.Personas.Add(persona);
            int response =await contexto.SaveChangesAsync();
            if(response==1)
                {
                sw= true;
            }
            return sw;
        }

        public async Task<List<Persona>> ListarPersonaTodos()
        {
            var lista = await contexto.Personas.ToListAsync();
                return lista;
        }

        public async Task<bool> ModificarPersona(Persona persona, int id)
        {
            bool sw = false;
            Persona EditarPersona = await contexto.Personas.FindAsync(id);
            if(EditarPersona!=null)
            {
                //EditarPersona.CI=persona.CI;
                EditarPersona.Nombre=persona.Nombre;
                EditarPersona.Apellidos=persona.Apellidos;
                EditarPersona.FechaNacimiento=persona.FechaNacimiento;
                EditarPersona.Foto=persona.Foto;
                EditarPersona.Estado=persona.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<Persona> ObtenerPersonaById(int id)
        {
            Persona persona = await contexto.Personas.FirstOrDefaultAsync(x => x.Id == id);
            return persona;
        }
    }
}
