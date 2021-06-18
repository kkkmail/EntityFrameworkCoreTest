using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContext
{
    public class Quote
    {
        public Guid QuoteId { get; set; }
        public int SomeQuoteData { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ComputedColumn { get; private set; }

        public Guid QuotePropertyId { get; set; }
        public Guid? AnotherQuotePropertyId { get; set; }

        public QuoteProperty QuoteProperty { get; set; } = null!;
        public QuoteProperty? AnotherQuoteProperty { get; set; }
    }
}
