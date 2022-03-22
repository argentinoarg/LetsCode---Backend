using BACK.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BACK.Data
{
    public class KanbanDbContext : IdentityDbContext
    {
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options)
        {

        }

        public DbSet<CardModel> Cards { get; set; }
    }
}
