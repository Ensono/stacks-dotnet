namespace xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Entities;

public interface IEntity<TIdentity>
{
    TIdentity Id { get; }
}
