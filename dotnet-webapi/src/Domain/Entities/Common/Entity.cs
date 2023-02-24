using MassTransit;
using System;

namespace Domain.Entities.Common;

public abstract class Entity : Entity<Guid>
{
    public override Guid Id { get; set; } = NewId.NextSequentialGuid();
}

public abstract class Entity<T>
{
    public virtual T Id { get; set; } = default!;
}

