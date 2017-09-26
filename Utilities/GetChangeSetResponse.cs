using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Utilities
{
    public class Author
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class CheckedInBy
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Value
    {
        public int changesetId { get; set; }
        public string url { get; set; }
        public Author author { get; set; }
        public CheckedInBy checkedInBy { get; set; }
        public DateTime createdDate { get; set; }
        public string comment { get; set; }
        public bool? commentTruncated { get; set; }
    }

    public class GetChangeSetResponse: VisualStudio_Response
    {
        public int count { get; set; }
        public List<Value> value { get; set; }
    }
}
