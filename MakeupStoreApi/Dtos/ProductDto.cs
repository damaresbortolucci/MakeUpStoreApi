using System.Text.Json.Serialization;

namespace MakeupStoreApi.Dtos
{
    public class ProductDto
    {
        public string brand { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string image_link { get; set; }
        public string description { get; set; }
        public float rating { get; set; }
        public string category { get; set; }
        public string product_type { get; set; }
        public string[] tag_list { get; set; }
    }
}
