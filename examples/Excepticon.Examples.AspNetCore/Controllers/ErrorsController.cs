using System;
using Microsoft.AspNetCore.Mvc;

namespace Excepticon.Examples.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorsController
    {
        [HttpGet]
        public IActionResult Get()
        {
            // Any errors thrown in the app during the processing of a request will be 
            // intercepted and sent to Excepticon.
            throw new ApplicationException("This error will be sent to Excepticon.");
        }
    }
}
