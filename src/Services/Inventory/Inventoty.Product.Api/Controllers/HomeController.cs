using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventoty.Product.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
