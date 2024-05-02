using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.FooterAndHeader
{
    public class HeaderFooterInfoVm
    {
        public HeaderFooterInfoVm()
        {
            SocialMediaLinks = new List<SocialMediaLinksVM>();
            Blogs = new List<BlogsVM>();
        }
        public string AboutUs { get; set; }
        public string MainLogo { get; set; }
        public string PhoneNumber { get; set; }
        public List<SocialMediaLinksVM> SocialMediaLinks { get; set; }

        public List<BlogsVM> Blogs { get; set; }

        public ContactUsInfoVM GetInTouch { get; set; }


    }
}
