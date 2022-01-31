using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Módulo_8.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Context
{
    public class ApplicationDbContext : DbContext                            // Es necesario para el options.UseSqlServer del ConfigureServices | 
    {                                // IdentityDbContext<ApplicationUser>   // Microsoft.EntityFrameworkCore.SqlServer
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // Propiedades
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

    }
}
