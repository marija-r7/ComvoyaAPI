using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class TripOnThisDateAlreadyExistsException : Exception
    {
        public TripOnThisDateAlreadyExistsException(DateTime DateFrom, DateTime DateTo) : base($"Trip already exists in date range: From: {DateFrom.Date} To: {DateTo.Date}.") { }
    }
}
