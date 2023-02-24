using Domain.Entities.Common;
using System;

namespace Domain.Auth.Interfaces;

public interface ISession
{
    public DateTime Now { get; }
}