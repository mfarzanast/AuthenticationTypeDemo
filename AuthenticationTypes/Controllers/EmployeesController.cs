using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTypes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = new List<string> { "John Doe", "Jane Smith", "Alice Johnson" };

            return Ok(employees);
        }
        [HttpGet("all")]
        public IActionResult GetAllEmployees()
        {
            return Ok(new List<string> { "John Doe", "Jane Smith", "Alice Johnson", "Bob Brown", "Charlie White" });
        }
    }
}
