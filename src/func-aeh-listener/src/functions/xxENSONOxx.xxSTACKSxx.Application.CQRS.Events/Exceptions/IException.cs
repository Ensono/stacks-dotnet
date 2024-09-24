namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Exceptions;

public interface IException
{
    int ExceptionCode { get; }
}
