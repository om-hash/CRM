namespace Pal.Core.Domains.Required
{
    public class RequiredFields : BaseEntity<int>
    {
        public string ReferenceType { get; set; }
        public string Required { get; set; }
    }
}
