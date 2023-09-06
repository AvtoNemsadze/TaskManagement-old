using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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

        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int? UserId { get; set; }

        // Define a foreign key to link a task with a team (nullable)
        [ForeignKey("TeamId")]
        [JsonIgnore]
        public Team? Team { get; set; }
        public int? TeamId { get; set; }

        public string? AttachFile { get; set; }

        [JsonIgnore]
        public ICollection<CommentEntity>? Comments { get; set; } = new List<CommentEntity>();
    }
}
