namespace xxENSONOxx.xxSTACKSxx.Shared.Domain
{
    public interface IEntity<TIdentity>
    {
        TIdentity Id { get; }
    }
}
