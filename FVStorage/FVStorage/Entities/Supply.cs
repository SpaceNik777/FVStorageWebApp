using System.Text.Json.Serialization;

namespace FVStorage.Entities
{
    public class Supply
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        [JsonIgnore]
        public virtual Product ProductModel { get; set; }
    }
}