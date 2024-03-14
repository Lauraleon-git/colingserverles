using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Telefono
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }

        [StringLength(maximumLength: 50)]
        public string NroTelefono { get; set; } 

        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;

        [ForeignKey("IdPersona")]
        public virtual Persona? Persona { get; set; } = null!;
    }
}
