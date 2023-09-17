using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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


        // Foreign key to link a task with a user (Creator)
        [ForeignKey("CreatedByUserId")]
        [JsonIgnore]
        public ApplicationUser CreatedByUser { get; set; }
        public int CreatedByUserId { get; set; }


        // Foreign key to link a task with a user
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int? UserId { get; set; }

        // Foreign key to link a task with a team
        [ForeignKey("TeamId")]
        [JsonIgnore]
        public Team? Team { get; set; }
        public int? TeamId { get; set; }

        public string? AttachFile { get; set; }

        [JsonIgnore]
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
