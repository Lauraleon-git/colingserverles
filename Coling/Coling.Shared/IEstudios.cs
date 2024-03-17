using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public interface IEstudios
    {
        public int IdAfiliado { get; set; }
        public string IdProfesion { get; set; }
        public string IdInstitucion { get; set; }
        public string Tipo { get; set; } 
        public string TituloRecido { get; set; }
        public int Anio { get; set; }
        public string Estado { get; set; }


    }
}
