using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class InterestNotFoundException : ApplicationException
    {
        public InterestNotFoundException(int id) : base($"Interest with id {id} not found.") { }
    }
}
