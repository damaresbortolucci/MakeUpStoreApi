using System.Text.Json.Serialization;

namespace MakeupStoreApi.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("image_link")]
        public string Image_Link { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("rating")]
        public object Rating { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("product_type")]
        public string Product_Type { get; set; }

        [JsonPropertyName("tag_list")]
        public List<string> Tag_List { get; set; }
    }
}
