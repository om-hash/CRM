using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Chat
{
    public class ChatInboxListDTO
    {
        public int InboxId { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }

        public DateTime LastMsgDate { get; set; }

        [StringLength(50)]
        public string LastMsgContent { get; set; }

        public ChatType ChatType { get; set; }

        [StringLength(50)]
        public string Ref1 { get; set; }
        public string Ref1Name { get; set; }

        [StringLength(50)]
        public string Ref2 { get; set; }
        public string Ref2Name { get; set; }

        public UserType SenderType { get; set; }
        public UserType ReceiverType { get; set; }

    }
}
