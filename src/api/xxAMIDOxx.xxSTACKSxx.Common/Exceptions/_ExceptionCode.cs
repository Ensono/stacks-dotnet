﻿namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    /// <summary>
    /// Contants list of possible exceptions ids raised by the system
    /// Use this list to map an exceptio to 
    /// </summary>
    public enum ExceptionCode
    {
        // BASIC EXCEPTION CODES (HTTP related codes)
        UnauthorizedOperation = 401,
        ForbiddenOperation = 403,
        BadRequest = 400,
        NotFound = 404,
        Conflict = 409,


        // GENERIC EXCEPTION CODES (Generic for application and infrastructe related issues)

        /// <summary>
        /// The feature exist but is disabled by FeatureFlags
        /// </summary>
        FeatureDisabled = 1000,

        /// <summary>
        /// An operation failed because the circuit breaker is open and the request can't be processed
        /// </summary>
        CircuitBreakerEnabled = 1001,


        //SPECIFIC EXCEPTION CODES (Use values above 10000 to make easier to create codes in between)
        //MENU
        MenuAlreadyExists = 10409,
        MenuDoesNotExist = 10404,
        //CATEGORY
        CategoryAlreadyExists = 11409,
        CategoryDoesNotExist = 11404,
        //MENUITEM
        MenuItemAlreadyExists = 12409,
        MenuItemDoesNotExist = 12404,
        MenuItemPriceMustNotBeZero = 12500
    }
}
