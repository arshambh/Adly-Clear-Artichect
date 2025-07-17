using Adly.Domain.Entities.Ad;

namespace Adly.Domain.Common;


public interface IEntity
{

     DateTime CreateDate { get; set; }
     DateTime ModifiedDate { get; set; }

}

public class BaseEntity<TKey>: IEntity
{
    public DateTime CreateDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public TKey Id { get; protected set; }


    public override bool Equals(object? entity)
    {
        if (entity is null)
            return false;

        if (entity is not BaseEntity<TKey> baseEntity)
            return false;

        if (ReferenceEquals(this, entity))
            return true;

        return baseEntity.Id.Equals(this.Id);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }


    public static bool operator ==(BaseEntity<TKey>? left, BaseEntity<TKey>? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity<TKey>? left, BaseEntity<TKey>? right)
    {
        return !(left == right);
    }
}









