using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_8.Models
{
    public class AutorCreacionDTO
    {

        [Required]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
    }
}
