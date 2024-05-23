using ApiCrud.Estudantes;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data
{
    public class AppDbContext : DbContext
    {
        private DbSet<Estudante> Estudantes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Data Source=Banco.sqlite");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
