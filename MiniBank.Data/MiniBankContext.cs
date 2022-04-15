using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniBank.Data.BankAccounts;
using MiniBank.Data.Transactions;
using MiniBank.Data.Users;

namespace MiniBank.Data
{
    public class MiniBankContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }

        public DbSet<BankAccountDbModel> BankAccounts { get; set; }

        public DbSet<TransactionDbModel> Transactions { get; set; }

        public MiniBankContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniBankContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSnakeCaseNamingConvention()
                .UseLazyLoadingProxies();
        }
    }

    public class Factory : IDesignTimeDbContextFactory<MiniBankContext>
    {
        public MiniBankContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("FakeConnectionStringOnlyForMigrations")
                .Options;

            return new MiniBankContext(options);
        }
    }
}