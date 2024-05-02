using Pal.Core.Enums.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Languages
{
    public class LanguageCreateDTO
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Shortcut { get; set; }
        [StringLength(10)]
        public string Culture { get; set; }
        public bool IsRtl { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public List<LanguageStringResourceDTO> LanguageStringResourceDTOs { get; set; }
    }
    public class LanguageStringResourceDTO
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        [StringLength(100)]
        public string ResourceName { get; set; }
        [StringLength(500)]
        public string ResourceValue { get; set; }
        [StringLength(50)]
        public StringResourceGroup StringResourceGroup { get; set; }
        public string TextStringResourceGroup { get; set; }
    }
}
