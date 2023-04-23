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
        private List<string> _errors;

        public Validation()
        {
            _errors = new List<string>();
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public bool Validate(string path)
        {           
            if (string.IsNullOrEmpty(path))
            {
                _errors.Add($"This path {path} is null.");
            }
            return !_errors.Any();
        }
    }
}
