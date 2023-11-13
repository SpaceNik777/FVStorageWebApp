using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FVStorage.Entities
{
    public class Product
    {
        public Product() {
            Supplies = new HashSet<Supply>();
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public string Price { get; set; }
        public string SupplierCode { get; set; }
        
        public virtual Supplier Supplier { get; set; }
        [JsonIgnore]
        public virtual ICollection<Supply> Supplies { get; set; }
    }
}