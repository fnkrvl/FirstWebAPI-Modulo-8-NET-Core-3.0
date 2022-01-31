using Microsoft.AspNetCore.Mvc;
using Módulo_8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_8.Helpers
{
    public class GeneradorEnlaces
    {

        private IUrlHelper _urlHelper;
        
        public GeneradorEnlaces(IUrlHelper urlHelper)
        {
            this._urlHelper = urlHelper;
        }
         
        public ColeccionDeRecursos<AutorDTO> GenerarEnlaces(List<AutorDTO> autores)
        {
            var resultado = new ColeccionDeRecursos<AutorDTO>(autores);
            autores.ForEach(a => GenerarEnlaces(a));
            resultado.Enlaces.Add(new Enlace(_urlHelper.Link("ObtenerAutores", new { }), rel: "self", metodo: "GET"));
            resultado.Enlaces.Add(new Enlace(_urlHelper.Link("CrearAutor", new { }), rel: "crear-autor", metodo: "POST"));
            return resultado;
        }  

        public void GenerarEnlaces(AutorDTO autor)
        {
            autor.Enlaces.Add(new Enlace(_urlHelper.Link("ObtenerAutor", new { id = autor.ID }), rel: "self", metodo: "GET"));
            autor.Enlaces.Add(new Enlace(_urlHelper.Link("ActualizarAutor", new { id = autor.ID }), rel: "actualizar-autor", metodo: "POST"));
            autor.Enlaces.Add(new Enlace(_urlHelper.Link("BorrarAutor", new { id = autor.ID }), rel: "borrar-autor", metodo: "DELETE"));
        }

    }
}
