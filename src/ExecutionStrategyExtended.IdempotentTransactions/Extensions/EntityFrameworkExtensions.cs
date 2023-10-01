using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using ExecutionStrategyExtended.IdempotentTransactions.IdempotenceToken;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.IdempotentTransactions.Extensions;

public static class EntityFrameworkExtensions
{
    internal static DbSet<IdempotencyToken> IdempotencyTokens(this DbContext context)
    {
        return context.Set<IdempotencyToken>();
    }
    
    public static void AddIdempotencyTokensTable(this ModelBuilder builder, IdempotencyTokenTableConfiguration? options = null)
    {
        options ??= new IdempotencyTokenTableConfiguration();
        
        builder.Entity<IdempotencyToken>(typeBuilder =>
        {
            typeBuilder.HasKey(x => x.Id).HasName(options.PrimaryKeyConstraintName);
            typeBuilder.Property(x => x.Id).HasMaxLength(options.MaxLength);
            RelationalEntityTypeBuilderExtensions.ToTable(typeBuilder, (string?)options.TableName);
        });
    }
}