using UserAccessManagement.Domain.Commom;

namespace UserAccessManagement.Application.Models;

public class EntityModel<T> where T : struct, IComparable, IEquatable<T>
{
    public EntityModel(Entity<T> entity)
    {
        Id = entity.Id;
        Active = entity.Active;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public T Id { get; private set; }

    public bool Active { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }
}