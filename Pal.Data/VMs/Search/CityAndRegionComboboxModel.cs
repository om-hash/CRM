using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Search
{
    public enum ItemType
    {
        Country,
        City,
        Region,
        Neighborhood,
    }
    public class AddressSearchComboboxModel
    {
        public AddressSearchComboboxModel()
        {
            
        }

        public AddressSearchComboboxModel(int id, string name, ItemType itemType)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Parents { get; set; }

        public ItemType ItemType {get;set;}
    }
}
