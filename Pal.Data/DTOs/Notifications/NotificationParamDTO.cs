using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Notifications
{
    public class NotificationParamDTO
    {
        public NotificationParamDTO()
        {
        }

        public NotificationParamDTO(string param)
        {
            Param = param;
        }

        public long Id { get; set; }
        public long NotificationId { get; set; }
        public string Param { get; set; }
    }
}
