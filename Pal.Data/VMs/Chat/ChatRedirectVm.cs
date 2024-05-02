using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Chat
{
   public class ChatRedirectVm
    {
        //public ChatRedirectVm(bool IsRedirectTochart, ChatType? ChatType, UserType? ReceiverType, int? ReceiverId, string Name)
        //{
        //    this.IsRedirectTochart = IsRedirectTochart;
        //    this.ChatType = ChatType;
        //    this.ReceiverType = ReceiverType;
        //    this.ReceiverId = ReceiverId;
        //    this.Name = Name;
        //}
        public bool IsRedirectTochart { get; set; }
        public ChatType? ChatType { get; set; }
        public UserType? ReceiverType { get; set; }
        public int? ReceiverId { get; set; }

        public string Name { get; set; }
        public string ReceiverName { get; set; }
    }
}
