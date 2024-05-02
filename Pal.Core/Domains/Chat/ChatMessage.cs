using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Chat
{
    public class ChatMessage
    {
        public Guid Id { get; set; }

        public int InboxId { get; set; }

        public UserType SenderType { get; set; }
        public UserType ReceiverType { get; set; }


        public MsgType MsgType { get; set; }

        public string Content { get; set; }

        public DateTime MsgDateLocal { get; set; }

        public DateTime MsgDateUtc { get; set; }


        //-----------------------------------
        [ForeignKey(nameof(InboxId))]
        public virtual ChatInbox ChatInbox { get; set; }
    }
}
