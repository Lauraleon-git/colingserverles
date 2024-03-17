using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class ProfesionAfiliado
    {
        [Key]
        public int Id { get; set; }
        public int IdAfiliado { get; set; }
        public string IdProfesion { get; set; }

        public DateTime FechaAsignacion { get; set; }

        [StringLength(maximumLength: 200)]
        public string NroSelloSib { get; set; } 

        [StringLength(maximumLength: 20)]
        public string Estado { get; set; }

        [ForeignKey("IdAfiliado")]
        public virtual Afiliado? Afiliado { get; set; } = null!;

        
    }
}
