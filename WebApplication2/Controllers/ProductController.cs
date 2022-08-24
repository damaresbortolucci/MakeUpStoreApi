using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MakeupStoreApi.Dtos;
using MakeupStoreApi.Models;
using MakeupStoreApi.Repository;

namespace MakeupStoreApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            this._logger = logger;
        }



        //API usada como base "https://makeup-api.herokuapp.com/"


        //BUSCA PAGINADA por product_type
        //EX: blush, bronzer, lipstick, eyebrow, nail_polish, mascara, foundation

        [HttpGet("type")]
        public ActionResult<IEnumerable<Product>> Get([FromQuery] string type, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var data = ProductRepository.GetData();

            if (!string.IsNullOrWhiteSpace(type))
            {
                var filteredData = data
                    .Where(x => x.Product_Type == type)
                    .OrderBy(x=>x.Brand)
                    .ThenBy(x=>x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList();
                return Ok(filteredData);
            }

            var result = data
                .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Consulta bem sucedida",
                Meta = new
                {
                    CurrentPage = page,
                    PageSize = pageSize
                },
                Data = result
            });
        }



        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var data = ProductRepository.GetData();
            var filteredData = data.Where(x => x.Id == id).FirstOrDefault();

            if (filteredData == null)
            {
                return NotFound(new
                {
                    message = "ID não encontrado"
                });
            }
            return Ok(filteredData);
        }


        //Busca todas as marcas e o total de produtos em cada uma
        [HttpGet("allBrands")]
        public ActionResult<List<string>> GetAllBrands()
        {
            var data = ProductRepository.GetData();

            var filteredData = data
                .Where(x => x.Brand != null)
                .OrderBy(x=>x.Brand)
                .GroupBy(x => x.Brand)
                .Select(x => new
                {
                    Marca = x.Key,
                    Total_Produtos = x.Key.Count()
                })
                .ToList();

            return Ok(filteredData);
        }


        //busca por marca específica
        // EX: maybelline, nyx, l'oreal, milani, revlon

        [HttpGet("brand")]
        public ActionResult<IEnumerable<Product>> GetByBrandName([FromQuery] string brand)
        {
            var data = ProductRepository.GetData();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                var brandToLower = brand.ToLower();
                var filteredData = data
                    .Where(x => x.Brand == brandToLower)
                    .OrderBy(x => x.Id)
                    .ToList();

                return Ok(filteredData);
            }

           return NotFound(new
                {
                    message = "Marca não encontrada"
                });
        }



        [HttpPost]
        public object Create([FromBody] ProductDto prod)
        {
            var data = ProductRepository.GetData();
            var lastId = data.OrderBy(p => p.Id).Last().Id + 1;

            var newProduct = new Product
            {
                Id = lastId,
                Brand = prod.brand,
                Name = prod.name,
                Price = prod.price,
                Image_Link = prod.image_link,
                Description = prod.description,
                Rating = prod.rating,
                Category = prod.category,
                Product_Type = prod.product_type,
                Tag_List = prod.tag_list
            };

            data.Add(newProduct);
            var content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText("./data.json", content);

            return Created($"https://localhost:44332/users/{lastId}", newProduct);
        }




        [HttpPut]
        public ActionResult<Product> Update(int id, [FromBody] ProductDto prod)
        {
            var data = ProductRepository.GetData();
            var productToUpdate = data.SingleOrDefault(p => p.Id == id);

            if (productToUpdate == null)
                return NotFound("ID não localizado");


            productToUpdate.Brand = prod.brand;
            productToUpdate.Name = prod.name;
            productToUpdate.Price = prod.price;
            productToUpdate.Image_Link = prod.image_link;
            productToUpdate.Description = prod.description;
            productToUpdate.Rating = prod.rating;
            productToUpdate.Category = prod.category;
            productToUpdate.Product_Type = prod.product_type;
            productToUpdate.Tag_List = prod.tag_list;

            var content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText("./data.json", content);

            return Ok("Produto atualizado");
        }



        [HttpDelete("id")]
        public ActionResult<string> Delete(int id)
        {
            var data = ProductRepository.GetData();
            var productoToDelete = data.SingleOrDefault(p => p.Id == id);

            if (productoToDelete == null)
                return NotFound("ID não localizado");

            if (data.Remove(productoToDelete))
            {
                var content = JsonSerializer.Serialize(data);
                System.IO.File.WriteAllText("./data.json", content);
                return Ok("Exclusão bem sucedida");
            }
            return BadRequest("Houve um erro e o produto não foi excluído.");
        }
    }
}