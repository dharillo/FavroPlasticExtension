using System.Collections.Generic;

namespace FavroPlasticExtensionTests.Helpers
{
    public class PagedResponse<TEntry>
    {
        /// <summary>
        /// Identifer of the request
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// Index of the page
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Number of pages
        /// </summary>
        public int Pages { get; set; }
        /// <summary>
        /// Limit of entries in the list
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// List of entries returned in this page
        /// </summary>
        public List<TEntry> Entries { get; set; }
    }
}
