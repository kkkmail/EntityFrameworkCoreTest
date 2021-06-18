using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContext
{
    public class Quote
    {
        public Guid QuoteId { get; set; }
        public decimal SomeQuoteData { get; set; }
        public decimal SomeMoreQuoteData { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ComputedColumn { get; private set; }

        public Guid QuotePropertyId { get; set; }
        public Guid? AnotherQuotePropertyId { get; set; }

        public QuoteProperty QuoteProperty { get; set; } = null!;
        public QuoteProperty? AnotherQuoteProperty { get; set; }
    }
}
