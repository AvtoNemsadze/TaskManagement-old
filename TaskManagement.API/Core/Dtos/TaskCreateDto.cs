using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.API.Core.Enums;

namespace TaskManagement.API.Core.Dtos
{
    public class TaskCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        //[EnumDataType(typeof(Enums.TaskStatus))]
        //[MaxLength(50)]
        //public string Status { get; set; }

        [EnumDataType(typeof(TaskPriority))]
        public string Priority { get; set; }
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
