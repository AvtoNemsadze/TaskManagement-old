using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagement.API.Core.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [ForeignKey("TaskId")]
        [JsonIgnore]
        public TaskEntity? Task { get; set; }
        public int? TaskId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int? UserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
