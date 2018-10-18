using System;

namespace StatSnapShotter.Entities
{
    public class QueryParams
    {
        public string query { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
