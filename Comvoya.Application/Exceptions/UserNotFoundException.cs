using ComvoyaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId) : base($"User with id {userId} not found.") { }
        public UserNotFoundException(string username) : base($"User with username {username} not found.") { }
    }
}
