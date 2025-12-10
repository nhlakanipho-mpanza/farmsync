using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Infrastructure.Data.Configurations;

public class TaskChecklistProgressConfiguration : IEntityTypeConfiguration<TaskChecklistProgress>
{
    public void Configure(EntityTypeBuilder<TaskChecklistProgress> builder)
    {
        builder.ToTable("TaskChecklistProgress");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.CompletionNotes)
            .HasMaxLength(500);
        
        builder.HasOne(p => p.WorkTask)
            .WithMany(w => w.ChecklistProgress)
            .HasForeignKey(p => p.WorkTaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.TaskChecklistItem)
            .WithMany(c => c.ChecklistProgress)
            .HasForeignKey(p => p.TaskChecklistItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.CompletedByEmployee)
            .WithMany(e => e.CompletedChecklistItems)
            .HasForeignKey(p => p.CompletedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
