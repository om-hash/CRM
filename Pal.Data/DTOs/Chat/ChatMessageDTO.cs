using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Chat
{
    public class ChatMessageDTO
    {
        public Guid Id { get; set; }

        public int InboxId { get; set; }

        public ChatType ChatType { get; set; }

        /// <summary>
        /// رقم الشركة او رقم العميل ...الخ
        /// </summary>
        [StringLength(50)]
        public string ReferenceId1 { get; set; }

        /// <summary>
        /// رقم الشركة او رقم العميل ...الخ
        /// </summary>
        [StringLength(50)]
        public string ReferenceId2 { get; set; }

        [StringLength(50)]
        public string SenderId { get; set; }
        public UserType SenderType { get; set; }
        public UserType ReceiverType { get; set; }

        [StringLength(50)]
        public string ReceiverId { get; set; }


        public MsgType MsgType { get; set; }


        public string Content { get; set; }

        public DateTime MsgDateLocal { get; set; }

        public DateTime MsgDateUtc { get; set; }

        [StringLength(50)]
        public string ContactName { get; set; }



    }
}
