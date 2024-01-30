using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    //Heatl checker - verifica se a API está online
    [HttpGet]
    [Route("")]
    public IActionResult Get()
        => Ok(new { message = "API rodando" });
    

}
