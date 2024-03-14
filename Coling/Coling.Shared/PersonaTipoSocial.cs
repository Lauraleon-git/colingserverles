using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class PersonaTipoSocial
    {
        [Key]
        public int Id { get; set; }
        public int IdTipoSocial { get; set; }
        public int IdPersona { get; set; }

        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } 

        [ForeignKey("IdPersona")]
        public virtual Persona? Persona { get; set; } = null!;

        [ForeignKey("IdTipoSocial")]
        public virtual TipoSocial? TipoSocial { get; set; } = null!;
    }
}
