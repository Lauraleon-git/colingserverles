using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Direccion
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }
        [StringLength(maximumLength: 100)]
        public string Descripcion {  get; set; }
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; }
        [ForeignKey("IdPersona")]
        public virtual Persona? Persona { get; set; } = null!;

    }
}
