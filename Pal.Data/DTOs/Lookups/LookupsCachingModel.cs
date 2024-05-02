using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Lookups
{
    public class LookupsCachingModel
    {
        public int Id { get; set; }

        public int ParentId { get; set; }
        public string Name { get; set; }
        public ICollection<LookupsTranslateCachingModel> Translates { get; set; }

    }

    public class LookupsTranslateCachingModel
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public string Name { get; set; }
    }
}
