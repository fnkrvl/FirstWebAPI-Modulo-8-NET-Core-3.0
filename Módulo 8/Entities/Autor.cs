using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_8.Entities
{
    public class Autor
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public List<Libro> Books { get; set; }

    }
}
