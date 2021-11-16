using Core.stam;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class StamDbContext : DbContext
    {
        public StamDbContext(DbContextOptions<StamDbContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Operation> Operations { get; set; }
    }
}