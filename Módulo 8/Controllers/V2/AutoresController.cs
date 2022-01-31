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
    [Route("api/v2/[controller]")]
    [ApiController]
    //[HttpHeaderIsPresent("x-version", "2")]
    public class AutoresController : ControllerBase
    {

        public AutoresController(ApplicationDbContext context)  // Inyectamos el servicio de AutoMapper
        {
            this.context = context; 
        }

        [HttpGet(Name = "ObtenerAutoresV2")]
        [ServiceFilter(typeof(HATEOASAuthorsFilterAttribute))]
        public async Task<ActionResult<IEnumerable<Autor>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);

            return Ok(autoresDTO);
        }

    }



}
