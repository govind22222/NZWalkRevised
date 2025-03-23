using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalkRevise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        public StudentsController()
        {
                
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            string[] students = new string[] { "John", "Jane", "Doe" };
            return Ok(students);
        }
    }
}
