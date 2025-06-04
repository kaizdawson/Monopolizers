using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Monopolizers.Repository.DB
{
    public class CardARContext : IdentityDbContext<ApplicationUser>
    {
        public CardARContext(DbContextOptions<CardARContext> opt):base(opt) 
        {


        }
        #region DbSet

        public DbSet<Card>? Cards { get; set; }
        public DbSet<TypeCard>? Types { get; set; }
        public DbSet<Design>? Designs { get; set; }
        public DbSet<Invite>? Invites { get; set; }
        public DbSet<Wallet>? Wallets { get; set; }
        public DbSet<WalletTransaction>? WalletTransactions { get; set; }
        public DbSet<PricingPlans>? PricingPlans { get; set; }
        public DbSet<PlanPurchase>? PlanPurchases { get; set; }
        public DbSet<Asset>? Assets { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PricingPlans>(entity =>
            {
                entity.HasKey(p => p.PricingPlansId); 
            });
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<ApplicationUser>(u => u.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
    .HasOne(u => u.PricingPlans)
    .WithMany(p => p.Users)
    .HasForeignKey(u => u.PricingPlansId)
    .OnDelete(DeleteBehavior.NoAction);





            modelBuilder.Entity<PlanPurchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.PlanPurchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Design>()
                .HasOne(d => d.User)
                .WithMany(u => u.Designs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.User)
                .WithMany(u => u.Assets)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
