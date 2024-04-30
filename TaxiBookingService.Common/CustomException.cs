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

        public class CannotCancelException : Exception
        {
            public CannotCancelException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class EmailAlreadyExistsExecption : Exception
        {
            public EmailAlreadyExistsExecption(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class NotStartedException : Exception
        {
            public NotStartedException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class InvalidverificationPinExecption : Exception
        {
            public InvalidverificationPinExecption(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class CannotUpdateDropOffExecption : Exception
        {
            public CannotUpdateDropOffExecption(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class RideAlreadyAcceptedExecption : Exception
        {
            public RideAlreadyAcceptedExecption(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class PaymentNotCompletedExecption : Exception
        {
            public PaymentNotCompletedExecption(string message, ILoggerAdapter loggerAdapter) : base(message)
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

        public class RideNotCompletedException : Exception
        {
            public RideNotCompletedException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class ScheduleDateNotProvidedException : Exception
        {
            public ScheduleDateNotProvidedException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class CannotUpdateStatusExecption : Exception
        {
            public CannotUpdateStatusExecption(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class CustomerAlreadyInSearchRideException : Exception
        {
            public CustomerAlreadyInSearchRideException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class InsufficientFundsException : Exception
        {
            public InsufficientFundsException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

        public class InvalidTopUpAmountException : Exception
        {
            public InvalidTopUpAmountException(string message, ILoggerAdapter loggerAdapter) : base(message)
            {
                loggerAdapter.LogInformation(message);
            }
        }

    }
}
