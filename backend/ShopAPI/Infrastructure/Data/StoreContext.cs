using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions context) 
            : base(context)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
