using System;
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro.API
{
    public class PagedResponse<TItem>
    {
        public int Page { get; private set; }

        public int Pages { get; private set; }

        public string RequestId { get; private set; }

        public List<TItem> Entries { get; private set; }
    }
}
