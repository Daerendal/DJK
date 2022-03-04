using Merkado.Models;
using Microsoft.EntityFrameworkCore;

namespace Merkado.DAL
{
    public class MerkadoDbContext : DbContext
    {


        public MerkadoDbContext(DbContextOptions<MerkadoDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
