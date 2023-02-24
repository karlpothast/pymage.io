using Application.Common;
using Domain.Entities;
using Infrastructure.Configuration;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbContext : DbContext, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseExceptionProcessor();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
    }
}