using System.Text.Json;
using MakeupStoreApi.Models;

namespace MakeupStoreApi.Repository
{
    public static class ProductRepository
    {
        public static List<Product> GetData()
        {
            var reader = new StreamReader("./data.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<Product>>(json);
            return data;
        }
    }
}
