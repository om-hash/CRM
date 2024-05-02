using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysPaymentType : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysPaymentTypeTranslate> SysPaymentTypeTranslates { get; set; }
    }

    public class SysPaymentTypeTranslate : BaseEntityTranslate<int>
    {
        public int PaymentTypeId { get; set; }


        [StringLength(50),Required]
        public string PaymentTypeName { get; set; }


        [ForeignKey(nameof(PaymentTypeId))]
        public virtual SysPaymentType SysPaymentType { get; set; }
    }
}
