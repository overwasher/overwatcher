using Microsoft.AspNetCore.Mvc;

namespace Overwatcher.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}