using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Infrastructure.Data.Configurations;

public class TaskChecklistItemConfiguration : IEntityTypeConfiguration<TaskChecklistItem>
{
    public void Configure(EntityTypeBuilder<TaskChecklistItem> builder)
    {
        builder.ToTable("TaskChecklistItems");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(c => c.Notes)
            .HasMaxLength(1000);
        
        builder.HasOne(c => c.TaskTemplate)
            .WithMany(t => t.ChecklistItems)
            .HasForeignKey(c => c.TaskTemplateId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(c => c.ChecklistProgress)
            .WithOne(p => p.TaskChecklistItem)
            .HasForeignKey(p => p.TaskChecklistItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
