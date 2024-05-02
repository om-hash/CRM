using Pal.Core.Enums.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Chat
{
    [Table(name: "ChatInbox")]
    public class ChatInbox
    {
        public int Id { get; set; }

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
        public string ReferenceName1 { get; set; }

        [StringLength(50)]
        public string ReferenceName2 { get; set; }

        [StringLength(20)]
        public string LastMsgContent { get; set; }

        public int EmployeeId { get; set; }
        /// <summary>
        /// UTC
        /// </summary>
        public DateTime LastMsgDate { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
