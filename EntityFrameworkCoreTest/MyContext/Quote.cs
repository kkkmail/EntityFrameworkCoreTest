using System;

namespace MyContext
{
    public class Quote
    {
        public Guid QuoteId { get; set; }
        public int SomeQuoteData { get; set; }

        public Guid QuotePropertyId { get; set; }
        public Guid? AnotherQuotePropertyId { get; set; }

        public QuoteProperty QuoteProperty { get; set; } = null!;
        public QuoteProperty? AnotherQuoteProperty { get; set; }
    }
}
