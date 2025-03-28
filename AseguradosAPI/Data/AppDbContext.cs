using Microsoft.EntityFrameworkCore;
using AseguradosAPI.Models;

namespace AseguradosAPI.Data
{
    // Clase base de datos
    public class AppDbContext : DbContext
    {
        // Constructor de la clase base de datos con la opción de configuración
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representación de la tabla Asegurados en la base de datos
        public DbSet<Asegurado> Asegurados { get; set; }  
    }
}