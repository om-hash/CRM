using Pal.Core.Enums.Language;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Languages
{
    public class Language : BaseEntityNoIdentity<int>
    {
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Shortcut { get; set; }
        [StringLength(10)]
        public string Culture { get; set; }
        public bool IsRtl { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class LanguageStringResource
    {
        [Key]
        public int Id { get; set; }
        public int LanguageId { get; set; }
        [StringLength(100)]
        public string ResourceName { get; set; }
        [StringLength(500)]
        public string ResourceValue { get; set; }
        [StringLength(50)]
        public StringResourceGroup StringResourceGroup { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }
    }


    public class LanguageStringResourceSemplified 
    {
        public int LanguageId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }

    }

    /// <summary>
    /// Represents a localized property
    /// </summary>
    public partial class LocalizedProperty : BaseEntity<int>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the locale key group
        /// </summary>
        public string LocaleKeyGroup { get; set; }

        /// <summary>
        /// Gets or sets the locale key
        /// </summary>
        public string LocaleKey { get; set; }

        /// <summary>
        /// Gets or sets the locale value
        /// </summary>
        public string LocaleValue { get; set; }
    }
}
