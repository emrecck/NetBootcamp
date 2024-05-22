using Bootcamp.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Application.Products
{
    public interface IProductRepository
    {
        Task<Product> Create(Product product);
        Task<Product?> GetById(int id);
    }
}
