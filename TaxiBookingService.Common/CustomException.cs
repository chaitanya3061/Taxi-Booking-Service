using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Utilities;

namespace TaxiBookingService.Common
{
    public class CustomException
    {
        public class InvalidTokenException : Exception 
        {
            public InvalidTokenException(string message , ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }
        public class AuthenticationException : Exception
        {
            public AuthenticationException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }
        public class TokenExpiredException : Exception
        {
            public TokenExpiredException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }
        public class NotFoundException : Exception
        {
            public NotFoundException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }
        public class CannotCancel : Exception
        {
            public CannotCancel(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }
        public class EmailAlreadyExists : Exception
        {
            public EmailAlreadyExists(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }
        public class NotStarted : Exception
        {
            public NotStarted(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class NomatchesFound : Exception
        {
            public NomatchesFound(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class InvalidverificationPin : Exception
        {
            public InvalidverificationPin(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class CannotUpdateDropOff : Exception
        {
            public CannotUpdateDropOff(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class RideAlreadyAccepted : Exception
        {
            public RideAlreadyAccepted(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class PaymentNotCompleted : Exception
        {
            public PaymentNotCompleted(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class SameLocationException : Exception
        {
            public SameLocationException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

    }
}
