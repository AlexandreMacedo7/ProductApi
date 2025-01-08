using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Exceptions;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }


        [HttpPost]
        public IActionResult Create(ProductRequestDto product)
        {
            int newProductId = _productService.Create(product);
            var createProduct = _productService.GetById(newProductId);

            return CreatedAtAction(nameof(GetById), new { id = createProduct.Id }, createProduct);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductRequestDto product)
        {
            try
            {
                int productId = _productService.UpdateProduct(id, product);
                var updateProduct = _productService.GetById(productId);

                return Ok(new
                {
                    product = updateProduct,
                    location = Url.Action(nameof(GetById), new { id = updateProduct.Id })
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateFields(int id, ProductUpdateRequestDto product)
        {
            try
            {

                int productId = _productService.UpdateFields(id, product);
                var updateProduct = _productService.GetById(id);

                return Ok(new
                {
                    productId = updateProduct,
                    location = Url.Action(nameof(GetById), new { id = updateProduct.Id })
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult<List<ProductResponseDto>> Get()
        {
            return _productService.GetAllProducts();
        }

        [HttpGet("{id}")]
        public ActionResult<ProductResponseDto> GetById(int id)
        {
            try
            {
                return _productService.GetById(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.GetById(id);

                _productService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
