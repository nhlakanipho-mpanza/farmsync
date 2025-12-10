using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Infrastructure.Data.Configurations;

public class TaskTemplateConfiguration : IEntityTypeConfiguration<TaskTemplate>
{
    public void Configure(EntityTypeBuilder<TaskTemplate> builder)
    {
        builder.ToTable("TaskTemplates");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.Description)
            .HasMaxLength(1000);
        
        builder.Property(t => t.Category)
            .HasMaxLength(100);
        
        builder.Property(t => t.EstimatedHours)
            .HasPrecision(18, 2);
        
        builder.Property(t => t.RecurrencePattern)
            .HasMaxLength(50);
        
        builder.Property(t => t.Instructions)
            .HasMaxLength(2000);
        
        builder.HasMany(t => t.ChecklistItems)
            .WithOne(c => c.TaskTemplate)
            .HasForeignKey(c => c.TaskTemplateId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(t => t.WorkTasks)
            .WithOne(w => w.TaskTemplate)
            .HasForeignKey(w => w.TaskTemplateId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
