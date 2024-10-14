namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

public class MenuCreatedEvent
{
    public int EventCode { get; set; }

    public int OperationCode { get; set; }

    public Guid CorrelationId { get; set; }

    public Guid MenuId { get; set; }
}
