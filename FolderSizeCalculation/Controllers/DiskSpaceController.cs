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
        private IDiskSpaceProcessor _diskSpaceProc;
        public DiskSpaceController(IDiskSpaceProcessor diskSpaceProc)
        {
            _diskSpaceProc = diskSpaceProc;
        }

        [HttpGet("{path}")]
        public List<FileInfo> GetFiles(string path)
        {
            var info = _diskSpaceProc.GetFiles(path);
            return info;
        }
    }
}
