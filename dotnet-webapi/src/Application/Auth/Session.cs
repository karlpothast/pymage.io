using Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using ISession = Domain.Auth.Interfaces.ISession;

namespace Application.Auth;

public class Session : ISession
{
    public DateTime Now => DateTime.Now;


}