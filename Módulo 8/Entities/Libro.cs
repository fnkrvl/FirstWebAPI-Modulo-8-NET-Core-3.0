using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_8.Entities
{
    public class Libro
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public int AutorID { get; set; }
        public Autor Autor { get; set; }
    }
}
