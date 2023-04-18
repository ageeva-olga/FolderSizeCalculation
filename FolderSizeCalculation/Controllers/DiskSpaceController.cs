using Logic.Interfaces;
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
        private IDiskSpace _diskSpace;
        public DiskSpaceController(IDiskSpace diskSpace)
        {
            _diskSpace = diskSpace;
        }

        [HttpGet("{path}")]
        public Dictionary<string, long> GetFiles(string path)
        {
            var info = _diskSpace.GetFiles(path);
            return info;
            //return Ok();
        }
    }
}
