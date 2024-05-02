using Pal.Core.Enums.Account;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Account
{
  
    [Table("AspNetPhoneVerificationTokens")]
    public class AspNetPhoneVerificationToken
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string UserId { get; set; }

        public PhoneVerificationTokenType VerificationTokenType { get; set; }

        [StringLength(20), Required]
        public string PhoneNumber { get; set; }

        [StringLength(250), Required]
        public string Code { get; set; }

        public DateTime SendDate { get; set; }


    }
}
