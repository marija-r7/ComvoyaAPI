using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class UserInterestNotFoundException : Exception
    {
        public UserInterestNotFoundException(Guid userId) : base($"No interests have been found for user {userId}.") { }
    }
}
