using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Customer
{
    public class ProfileSidebarViewModel
    {
        public string CustomerImage { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string ViewName { get; set; }

        public ProfileSidebarViewModel() { }

        public ProfileSidebarViewModel(string image)
        {
            CustomerImage = image;
        }
    }
}
