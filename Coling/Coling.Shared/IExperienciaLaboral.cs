using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public interface IExperienciaLaboral
    {
        public int IdAfiliado {  get; set; }
        public string IdInstitucion { get; set; }
        public string CargoTitulo { get; set; } 
        public DateTimeOffset FechaInicio { get; set; }
        public DateTimeOffset FechaFinal { get; set; }
        public string Estado {  get; set; }
    }
}
