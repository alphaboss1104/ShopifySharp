using System.Collections.Generic;

namespace ShopifySharp.Filters
{
    public class UnpaginatedListFilter<T> : Parameterizable, IUnpaginatedListFilter<T>
    {
        public IEnumerable<KeyValuePair<string, object>> ToQueryParameters()
        {
            return base.ToParameters();
        }
    }
}