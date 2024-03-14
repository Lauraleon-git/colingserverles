using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Profesion
    {
        [Key]
        public int Id { get; set; }

        [StringLength(maximumLength: 100)]
        public string NombreProfesion { get; set; } 
        public int IdGrado { get; set; }

        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } 

        [ForeignKey("IdGrado")]
        public virtual GradoAcademico? GradoAcademico { get; set; } = null!;

    }
}
