﻿using EntityFrameworkCore.ExecutionStrategyExtended;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Demo;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddIdempotencyTokensTable();
    }
}