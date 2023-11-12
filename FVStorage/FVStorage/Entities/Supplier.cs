using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FVStorage.Entities
{
    public class Supplier
    {   
        public Supplier() {
            Products = new HashSet<Product>();
        }

        public string Code { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}