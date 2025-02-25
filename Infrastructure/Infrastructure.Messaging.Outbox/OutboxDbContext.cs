using Infrastructure.Messaging.Outbox.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Messaging.Outbox;

public class OutboxDbContext : DbContext
{
    public OutboxDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OutboxMessage> Outbox { get; set; }
}