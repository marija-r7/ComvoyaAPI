using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class TripNotFoundException : Exception
    {
        public TripNotFoundException(Guid id) : base($"Trip with Id {id} has not been found.") { }

    }
}
