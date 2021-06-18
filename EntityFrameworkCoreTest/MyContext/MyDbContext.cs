using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MyContext
{
    public class MyDbContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; } = null!;
        public DbSet<QuoteProperty> QuoteProperties { get; set; } = null!;

        public static string SqlConnectionString { get; } =
            "Data Source=localhost;Initial Catalog=MyDatabase;Integrated Security=True;";

        public static DbContextOptions<MyDbContext> GetDbContextOptions(string connectionString) =>
            new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlServer(connectionString, x => x.UseNetTopologySuite())
                .Options;

        public static DbContextOptions<MyDbContext> GetInMemoryDbContextOptions(Guid dbId) =>
            new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase($"{nameof(MyDbContext)}_{dbId:N}")
                .EnableSensitiveDataLogging()
                .Options;

        public static string GetSqLiteConnectionString(Guid dbId) =>
            $"DataSource={nameof(MyDbContext)}_{dbId:N};mode=memory;cache=shared;Foreign Keys=False";

        public static (SqliteConnection KeepAliveConnection, DbContextOptions<MyDbContext> Options) GetSqLiteDbContextOptions(
            Guid dbId,
            Func<DbContextOptions<MyDbContext>, MyDbContext> creator)
        {
            var connectionString = GetSqLiteConnectionString(dbId);
            var keepAliveConnection = new SqliteConnection(connectionString);

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlite(keepAliveConnection, x => x.UseNetTopologySuite())
                .EnableSensitiveDataLogging()
                .Options;

            keepAliveConnection.Open();
            using var ctx = creator(options);

            if (!ctx.Database.EnsureCreated())
            {
                throw new InvalidOperationException("Can't create database.");
            }

            return (keepAliveConnection, options);
        }

        public MyDbContext() : base(GetDbContextOptions(SqlConnectionString))
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("Starting...");

            modelBuilder.Entity<Quote>()
                .HasKey(e => e.QuoteId);

            modelBuilder.Entity<Quote>()
                .Property(e => e.SomeQuoteData)
                // .HasPrecision(18, 2)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Quote>()
                .Property(e => e.SomeMoreQuoteData)
                // .HasPrecision(18, 2)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Quote>()
                .Property(e => e.ComputedColumn)
                // .HasPrecision(18, 2)
                .HasPrecision(18, 4)
                .HasComputedColumnSql("(case when [SomeQuoteData]>[SomeMoreQuoteData] then [SomeQuoteData] else [SomeMoreQuoteData] end)",
                    stored: true);

            modelBuilder.Entity<QuoteProperty>()
                .HasKey(e => e.QuotePropertyId);

            modelBuilder.Entity<Quote>()
                .HasOne(e => e.QuoteProperty)
                .WithMany()
                .HasForeignKey(e => e.QuotePropertyId)
                .HasConstraintName("FK1")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quote>()
                .HasOne(e => e.AnotherQuoteProperty)
                .WithMany()
                .HasForeignKey(e => e.AnotherQuotePropertyId)
                .HasConstraintName("FK2")
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
