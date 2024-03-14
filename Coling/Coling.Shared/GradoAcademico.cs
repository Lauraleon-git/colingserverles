using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class GradoAcademico
    {
        [Key]
        public int Id { get; set; }
        [StringLength(maximumLength: 100)]
        public string NombreGrado { get; set; } 

        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } 
    }
}
