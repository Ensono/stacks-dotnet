using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.Exceptions
{
    /// <summary>
    /// Contants list of possible exceptions ids raised by the system
    /// Use this list to map an exceptio to 
    /// </summary>
    public static class ExceptionIds
    {
        // BASIC EXCEPTION CODES (HTTP related codes)

        public const int UnauthorizedOperation = 401;
        public const int ForbiddenOperation = 403;
        public const int BadRequest = 400;
        public const int NotFound = 404;


        // GENERIC EXCEPTION CODES (Generic for application and infrastructe related issues)

        /// <summary>
        /// The feature exist but is disabled by FeatureFlags
        /// </summary>
        public const int FeatureDisabled = 1000; 
        /// <summary>
        /// An operation failed because the circuit breaker is open and the request can't be processed
        /// </summary>
        public const int CircuitBreakerEnabled = 1001; 


        //SPECIFIC EXCEPTION CODES (Use values above 10000)

        public const int MenuAlreadyExists = 10409;
        public const int MenuDoesNotExists = 10404;
    }
}
