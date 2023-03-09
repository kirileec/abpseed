using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.Swagger
{
    public interface ISwaggerHtmlResolver
    {
        Stream Resolver();
    }
}
