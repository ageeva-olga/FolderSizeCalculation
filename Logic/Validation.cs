using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Validation : IValidation
    {
        public string ValidateNotNull(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException($"This path {path} is null.");
            }
            else
            {
                return path;
            }
        }
    }
}
