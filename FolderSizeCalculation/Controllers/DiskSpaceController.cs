using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderSizeCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiskSpaceController : Controller
    {
        private IDiskSpaceProcessor _diskSpaceProc;
        public DiskSpaceController(IDiskSpaceProcessor diskSpaceProc)
        {
            _diskSpaceProc = diskSpaceProc;
        }

        [HttpGet("{path}")]
        public IActionResult GetFiles(string path)
        {           
            try
            {
                var info = _diskSpaceProc.GetFiles(path);
                return Ok(info);
            }

            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentNullException argNull)
            {
                return StatusCode(StatusCodes.Status400BadRequest, argNull.Message);
            }
        }
    }
}
