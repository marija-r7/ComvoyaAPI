using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class InterestAlreadyExistsException : Exception
    {
        public InterestAlreadyExistsException(string name) : base($"Interest with name '{name}' already exists.") { }
    }
}
