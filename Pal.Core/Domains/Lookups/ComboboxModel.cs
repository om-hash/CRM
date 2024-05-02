using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pal.Core.Domains.Lookups
{
    public class ComboboxModel
    {
        public ComboboxModel()
        {
        }

        public ComboboxModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }


    public class ComboboxModelTranslate
    {
        public ComboboxModelTranslate()
        {
        }

        public ComboboxModelTranslate(int id, string name)
        {

            Id = id;
            Name = name;
        }

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("LanguageId")]
        public int LanguageId { get; set; }

        [JsonPropertyName("Name")]
        [StringLength(100)]
        public string Name { get; set; }
    }

}
