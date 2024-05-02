using System.Collections.Generic;

namespace Pal.Data.DTOs.Required
{
    public class RequiredFieldsDTO
    {
        public List<string> Company { get; set; }
        public List<string> RealEstate { get; set; }
        public List<string> Project { get; set; }
        public List<string> Region { get; set; }
        public List<string> Employee { get; set; }
        public List<string> Task { get; set; }
        public List<string> Call { get; set; }
        public List<string> Advisor { get; set; }
    }
}
