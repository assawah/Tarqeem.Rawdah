namespace Tarqeem.CA.Domain.Common;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}