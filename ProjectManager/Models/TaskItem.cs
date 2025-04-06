using System;
using System.ComponentModel.DataAnnotations;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EstimatedEndDate { get; set; }

    [Required]
    public string Status { get; set; } // Enum yerine string olarak tanımlandı

    // Foreign Key
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
