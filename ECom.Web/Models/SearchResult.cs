
using DataTable;

namespace ECom.Web.Models
{
    public abstract class SearchResult<T> : SearchParameter<T>
    {
        public string odatacontext { get; set; }
        public int odatacount { get; set; }
        public T[]  value {get;set;}
    }
}
