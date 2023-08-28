using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.API.Core.Enums;

namespace TaskManagement.API.Core.Entities
{
    public class TaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; } 

        [EnumDataType(typeof(Enums.TaskStatus))]
        [MaxLength(50)]
        public string Status { get; set; }

        [EnumDataType(typeof(TaskPriority))]
        public string Priority { get; set; }
    }
}
