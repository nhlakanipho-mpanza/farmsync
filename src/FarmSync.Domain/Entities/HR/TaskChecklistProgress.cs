using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TaskChecklistProgress : BaseEntity
{
    public Guid WorkTaskId { get; set; }
    public Guid TaskChecklistItemId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public Guid? CompletedBy { get; set; } // Employee who completed this item
    public string? CompletionNotes { get; set; }

    // Navigation properties
    public virtual WorkTask WorkTask { get; set; } = null!;
    public virtual TaskChecklistItem TaskChecklistItem { get; set; } = null!;
    public virtual Employee? CompletedByEmployee { get; set; }
}
