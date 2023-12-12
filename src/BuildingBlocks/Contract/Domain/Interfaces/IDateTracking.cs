using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Domain.Interfaces
{
    public interface IDateTracking
    {
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? LassModifiedDate { get; set; }
    }
}
