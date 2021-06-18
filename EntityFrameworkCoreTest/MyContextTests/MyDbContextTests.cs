using System;
using MyContext;
using Xunit;

namespace MyContextTests
{
    public class MyDbContextTests
    {
        [Fact]
        public void SqlServer_AddEntities_ShouldWork()
        {
            using var ctx = new MyDbContext();
            AddEntities_ShouldWork(ctx);
        }

        [Fact]
        public void Sqlite_AddEntities_ShouldWork()
        {
            var (keepAlive, options) = MyDbContext.GetSqLiteDbContextOptions(Guid.NewGuid(), o => new MyDbContext(o));

            try
            {
                using var ctx = new MyDbContext(options);
                AddEntities_ShouldWork(ctx);
            }
            finally
            {
                keepAlive.Close();
            }
        }

        [Fact]
        public void InMemoryDatabase_AddEntities_ShouldWork()
        {
            var options = MyDbContext.GetInMemoryDbContextOptions(Guid.NewGuid());
            using var ctx = new MyDbContext(options);
            AddEntities_ShouldWork(ctx);
        }

        private static void AddEntities_ShouldWork(MyDbContext ctx)
        {
            var quoteProperty = new QuoteProperty
            {
                SomeQuotePropertyData = 1,
            };

            ctx.QuoteProperties.Add(quoteProperty);
            ctx.SaveChanges();

            var quote = new Quote
            {
                SomeQuoteData = 1,
                QuoteProperty = quoteProperty,
            };

            ctx.Quotes.Add(quote);
            ctx.SaveChanges();
        }
    }
}
