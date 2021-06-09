using System.Net;

namespace Snyk.Fixes.Common.Exceptions
{
    public static class ExceptionCodeToHttpStatusCodeConverter
    {
        internal static HttpStatusCode GetHttpStatusCode(int exceptionCode)
        {
            switch ((ExceptionCode)exceptionCode)
            {
                case Exceptions.ExceptionCode.UnauthorizedOperation:
                    return HttpStatusCode.Unauthorized;
                case Exceptions.ExceptionCode.ForbiddenOperation:
                    return HttpStatusCode.Forbidden;
                case Exceptions.ExceptionCode.BadRequest:
                    return HttpStatusCode.BadRequest;
                case Exceptions.ExceptionCode.NotFound:
                    return HttpStatusCode.NotFound;
                case Exceptions.ExceptionCode.Conflict:
                    return HttpStatusCode.Conflict;
                case Exceptions.ExceptionCode.FeatureDisabled:
                    return HttpStatusCode.NotFound;
                case Exceptions.ExceptionCode.CircuitBreakerEnabled:
                    return HttpStatusCode.ServiceUnavailable;

                //Business related
                case Exceptions.ExceptionCode.localhostAlreadyExists:
                case Exceptions.ExceptionCode.CategoryAlreadyExists:
                case Exceptions.ExceptionCode.localhostItemAlreadyExists:
                    return HttpStatusCode.Conflict;

                case Exceptions.ExceptionCode.localhostDoesNotExist:
                case Exceptions.ExceptionCode.CategoryDoesNotExist:
                case Exceptions.ExceptionCode.localhostItemDoesNotExist:
                    return HttpStatusCode.NotFound;

                case Exceptions.ExceptionCode.localhostItemPriceMustNotBeZero:
                default:
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}
