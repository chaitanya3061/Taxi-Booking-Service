using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Common
{
    public class CustomException
    {
        public class InvalidTokenException : Exception
        {
            public InvalidTokenException(string message) : base(message)
            {
            }
        }
        public class AuthenticationException : Exception
        {
            public AuthenticationException(string message) : base(message)
            {
            }
        }
        public class TokenExpiredException : Exception
        {
            public TokenExpiredException(string message) : base(message)
            {
            }
        }
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message)
            {


            }
        }
        public class CannotCancel : Exception
        {
            public CannotCancel(string message) : base(message)
            {


            }
        }

    }
}
