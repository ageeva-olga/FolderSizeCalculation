using Logic.Interfaces;
using Logic.Models;
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
        private IDiskSpaceRepository _diskSpace;
        public DiskSpaceController(IDiskSpaceRepository diskSpace)
        {
            _diskSpace = diskSpace;
        }

        [HttpGet("{path}")]
        public List<FileInfo> GetFiles(string path)
        {
            var info = _diskSpace.GetFiles(path);
            return info;
        }
    }
}
