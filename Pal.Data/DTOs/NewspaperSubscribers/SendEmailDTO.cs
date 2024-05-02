using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.NewspaperSubscribers
{
    public class SendEmailDTO
    {
        public string Subject { get; set; }

        public string Body { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
