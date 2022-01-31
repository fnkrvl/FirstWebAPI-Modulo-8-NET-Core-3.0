using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_8.Models
{
    public class AutorDTO : Recurso
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public List<LibroDTO> Books { get; set; }

    }
}
