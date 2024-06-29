﻿namespace UserAccessManagement.Domain.Commom;

public abstract class Entity<T> where T : struct, IComparable, IEquatable<T>
{
    public Entity()
    {
        Active = true;
        CreatedAt = DateTime.UtcNow;
    }

    public Entity(T id)
    {
        Id = id; 
    }

    public T Id { get; private set; }

    public bool Active { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    protected void Stamp() => UpdatedAt = DateTime.UtcNow;

    protected void Inactivate()
    {
        Active = false;
        Stamp();
    }
}