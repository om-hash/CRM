using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Sms
{
    public class SmsTemplate
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string TemplateName { get; set; }

        public virtual ICollection<SmsTemplateTranslate> Translates { get; set; }
    }


    public class SmsTemplateTranslate : BaseEntityTranslate<int>
    {
        public int TemplateId { get; set; }


        [Required, StringLength(500)]
        public string MessageTemplate { get; set; }


        [ForeignKey(nameof(TemplateId))]
        public virtual SmsTemplate SmsTemplate { get; set; }

    }
}
