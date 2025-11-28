namespace Tarqeem.CA.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    public IUserRefreshTokenRepository UserRefreshTokenRepository { get; }
    Task CommitAsync();
    ValueTask RollBackAsync();
}