using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Módulo_8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_8.Helpers
{
    public class HATEOASAuthorsFilterAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HATEOASAuthorsFilterAttribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces ?? throw new ArgumentNullException(nameof(GeneradorEnlaces));
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var incluirHATEOAS = DebeIncluirHATEOAS(context);

            if (!incluirHATEOAS)
            {
                await next();
                return;
            }

            var result = context.Result as ObjectResult;
            var model = result.Value as List<AutorDTO> ?? throw new ArgumentNullException("Se esperaba una instancia.");
            result.Value = generadorEnlaces.GenerarEnlaces(model);
            await next();
        }

    }
}
