using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class TripCapacityFullException : Exception
    {
        public TripCapacityFullException() : base("Trip capacity is full.") { }
    }
}
