using ProductApi.Daos;
using ProductApi.Dtos;
using ProductApi.Exceptions;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class ProductService
    {
        private readonly ProductDao _productDao;

        public ProductService(ProductDao productDao)
        {
            _productDao = productDao;
        }

        public int Create(ProductRequestDto product)
        {
            return _productDao.Create(product);
        }

        public int UpdateProduct(int id, ProductRequestDto product)
        {
            GetExistingProductById(id);
            return _productDao.UpdateProduct(id, product);
        }

        public int UpdateFields(int id, ProductUpdateRequestDto product)
        {
            GetExistingProductById(id);
            return _productDao.UpdateFields(id, product);
        }

        public List<ProductResponseDto> GetAllProducts()
        {
            var products = _productDao.GetAll();
            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();
        }

        public ProductResponseDto GetById(int id)
        {
            var product = GetExistingProductById(id);
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public void Delete(int id)
        {
            GetExistingProductById(id);
            _productDao.Delete(id);
        }

        private Product GetExistingProductById(int id)
        {
            var product = _productDao.GetById(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found!");
            }
            return product;
        }
    }
}
