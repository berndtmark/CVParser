using CVParser.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CVParser.Infrastructure.Database.Context;

public class DataBaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<PipelineRun> PipelineRun { get; set; }
    public DbSet<StageRun> StageRun { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PipelineRun>(builder =>
        {
            builder.ToTable("PipelineRun");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileName).HasMaxLength(1024);
        });

        modelBuilder.Entity<StageRun>(builder =>
        {
            builder.ToTable("StageRun");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Stage).HasMaxLength(64);
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32);
            builder.HasIndex(x => x.PipelineRunId);
            builder.HasOne<PipelineRun>().WithMany().HasForeignKey(x => x.PipelineRunId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
