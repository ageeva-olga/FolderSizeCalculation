using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderSizeCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiskSpaceController : Controller
    {
        public DiskSpaceController()
        { }

        [HttpGet("{path}")]
        public IActionResult GetFiles(string path)
        {
            return Ok();
        }
    }
}
