using Microsoft.AspNetCore.Mvc;
using ShopAPI.Errors;

namespace ShopAPI.Controllers
{
    [Route("fallback/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FallbackController : BaseApiController
    {
        public IActionResult FallbackError([FromRoute]int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
