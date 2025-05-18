using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Monopolizers.DB
{
    public class CardARContext : IdentityDbContext<ApplicationUser>
    {
        public CardARContext(DbContextOptions<CardARContext> opt):base(opt) 
        {


        }
        #region DbSet

        public DbSet<Card>? Cards { get; set; }
        #endregion
    }
}
