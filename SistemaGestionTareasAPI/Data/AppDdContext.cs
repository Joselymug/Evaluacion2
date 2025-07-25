using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaGestionTareas;

    public class AppDdContext : DbContext
    {
        public AppDdContext (DbContextOptions<AppDdContext> options)
            : base(options)
        {
        }

        public DbSet<SistemaGestionTareas.Proyecto> Proyecto { get; set; } = default!;

public DbSet<SistemaGestionTareas.Tarea> Tarea { get; set; } = default!;

public DbSet<SistemaGestionTareas.Usuario> Usuario { get; set; } = default!;
    }
