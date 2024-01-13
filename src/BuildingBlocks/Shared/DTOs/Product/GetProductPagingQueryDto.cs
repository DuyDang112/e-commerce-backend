using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Product
{
    public class GetProductPagingQueryDto : PagingRequestParameters
    {
        public string SearchTerm { get; set; }
        public long[]? Categories { get; set; }
    }
}
