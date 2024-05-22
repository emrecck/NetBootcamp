using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Application.Products
{
    public record ProductCreateRequestDto(string Name, decimal Price);
}
