using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulo_5.Context;
using Módulo_8.Entities;
using Módulo_8.Helpers;
using Módulo_8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Controllers.V2
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[HttpHeaderIsPresent("x-version", "1")]
    public class AutoresController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUrlHelper urlHelper;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IUrlHelper urlHelper)  // Inyectamos el servicio de AutoMapper
        {
            this.context = context;
            this.mapper = mapper;
            this.urlHelper = urlHelper;
        }


        [HttpGet("/listado")]
        [HttpGet("listado")]
        public ActionResult<IEnumerable<Autor>> GetListado()
        {
            return context.Autores.Include(x => x.Books).ToList();
        }


        [HttpGet("/Primer")]
        [HttpGet("Primer")]
        public ActionResult<Autor> GetPrimerAutor()
        {
            return context.Autores.FirstOrDefault();  // Devuelve el primer autor que encuentra
        }

        [HttpGet(Name = "ObtenerAutoresV1")]
        [ServiceFilter(typeof(HATEOASAuthorsFilterAttribute))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get(int numeroDePagina = 1, int cantidadDeRegistros = 10)
        {
            var query = context.Autores.AsQueryable();

            var totalDeRegistros = query.Count();

            var autores = await query 
                .Skip(cantidadDeRegistros * (numeroDePagina - 1))
                .Take(cantidadDeRegistros)
                .ToListAsync();

            Response.Headers["X-Total-Registros"] = totalDeRegistros.ToString();
            Response.Headers["X-Cantidad-Paáginas"] =
                ((int)Math.Ceiling((double)totalDeRegistros / cantidadDeRegistros)).ToString();
                // Paginación realizada 

            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);                         

            return Ok(autoresDTO);
        }

        [HttpGet("{id}", Name = "ObtenerAutor")]
        [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(typeof(AutorDTO), 200)]
        public async Task<ActionResult<AutorDTO>> GetAutor(int id)
        {
            var autor = await context.Autores.Include(x => x.Books).FirstOrDefaultAsync(x => x.ID == id);
            // Ésta línea es la que busca el o los recursos externos a la aplicación y es la qiue podría tardar más, y por ello se hace la acción asíncrona

            if (autor == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return autorDTO;
        }

        private void GenerarEnlaces(AutorDTO autor)
        {
            autor.Enlaces.Add(new Enlace(href: urlHelper.Link("ObtenerAutor", new { id = autor.ID }), rel: "self", metodo: "GET"));
            autor.Enlaces.Add(new Enlace(href: urlHelper.Link("ActualizarAutor", new { id = autor.ID}), rel: "update-autor", metodo: "PUT"));
            autor.Enlaces.Add(new Enlace(href: urlHelper.Link("BorrarAutor", new { id = autor.ID}), rel: "delete-author", metodo: "DELETE"));
        }


        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {          // FromBody -> Indica que la información del autor viene en el cuerpo de la petición HTTP 
            TryValidateModel(autorCreacion);  // Si se requiere volver a hacer las validaciones a un modelo (cumple con las validaciones de atributos) 
            var autor = mapper.Map<Autor>(autorCreacion);
            context.Add(autor); // Add -> EF
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autorDTO.ID }, autorDTO); // 201 Created | Objeto creado
        }


        [HttpPut("{id}", Name = "ActualizarAutor")]  // Actualización completa de un recurso | Todos los campos de la clase
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.ID = id;
            // Entry - EF                                                                // DB CONTEXT
            context.Entry(autorActualizacion).State = EntityState.Modified;              // EntityState.Added      - INSERT
            await context.SaveChangesAsync();                                            // EntityState.Modified   - UPDATE
            return NoContent(); // Código 204 - Valor modificado | No devuelve nada      // EntityState.Deleted    - DELETE
        }                                                                                // EntityState.Unchanged  - On SaveChanges()
                                                                                         // 204 - The server has successfully fulfilled the request and that there is no additional content to send in the response payload body. 

        [HttpPatch("{id}", Name = "ActualizarParcialmenteAutor")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            // From the database
            var autorDB = await context.Autores.FirstOrDefaultAsync(x => x.ID == id);

            if (autorDB == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorCreacionDTO>(autorDB);

            patchDocument.ApplyTo(autorDTO);

            var isValid = TryValidateModel(autorDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(autorDTO, autorDB);

            await context.SaveChangesAsync();
            return NoContent();  // 204 Status

        }


        /// <summary>
        /// Borra un comentario específico
        /// </summary>
        /// <param name="id">Id del elemento a borrar</param>
        [HttpDelete("{id}", Name = "BorrarAutor")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            var autorID = await context.Autores.Select(x => x.ID).FirstOrDefaultAsync(x => x == id);

            if (autorID == default) // Porque el campo ID es un entero
            {
                return NotFound();  // Hereda de ActionResult
            }

            context.Autores.Remove(new Autor { ID = autorID });  // Remove -> EF
            await context.SaveChangesAsync();
            return NoContent(); // Devuelve el autor que se eliminó
        }

    }
}
