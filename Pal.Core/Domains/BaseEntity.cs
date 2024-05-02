
using Microsoft.AspNetCore.Identity;
using Pal.Core.Domains.Languages;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains
{
    //--------------------------------------------------------------------
    public class BaseEntity<T> 
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

    }
   

    //--------------------------------------------------------------------
    public class BaseEntityWithLog<T> : ISoftDeletedEntity
    {
        public BaseEntityWithLog()
        {
            CreationDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
            IsDeleted = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        [ScaffoldColumn(false)]
        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string? CreatedBy { get; set; }

        [ScaffoldColumn(false), Column(TypeName = "datetime2")]
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string? ModifiedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    //--------------------------------------------------------------------
    public class BaseEntityNoIdentity<T> 
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public T Id { get; set; }

    }

    //--------------------------------------------------------------------
    public class BaseEntityTranslate<T> : BaseEntity<T>
    {
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }

    }

    //--------------------------------------------------------------------
    public class BaseEntityTranslateWithLog<T> : BaseEntityWithLog<T>
    {
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }

    }

    //--------------------------------------------------------------------
    public class BaseEntityIdentityUser : IdentityUser
    {
        public BaseEntityIdentityUser()
        {
            CreationDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
            IsDeleted = false;
            LastActivityDate = DateTime.Now;
        }

        [ScaffoldColumn(false)]
        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string? CreatedBy { get; set; }

        [ScaffoldColumn(false),Column(TypeName = "datetime2")]
        public DateTime? LastModifiedDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        [Column(TypeName = "datetime2")]
        public DateTime? DeletionDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string? DeletedBy { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "datetime2")]
        public DateTime? LastActivityDate { get; set; }

    }



    public interface ISoftDeletedEntity
    {
        [ScaffoldColumn(false)]
        [Column(TypeName = "datetime2")]
        public DateTime? DeletionDate { get; set; }

        [ScaffoldColumn(false)]
        public string? DeletedBy { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }
    }

}
