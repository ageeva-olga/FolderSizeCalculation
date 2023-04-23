using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IValidation
    {
        public bool Validate(string path);
        public List<string> GetErrors();
    }
}
