using Microsoft.EntityFrameworkCore;

namespace CiaDeTalentos.Dashboard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Candidato> Candidatos { get; set; }
    }
}
