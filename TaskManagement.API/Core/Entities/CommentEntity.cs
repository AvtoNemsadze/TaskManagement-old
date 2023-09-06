using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagement.API.Core.Entities
{
    public class CommentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [ForeignKey("TaskId")]
        [JsonIgnore]
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        public TaskEntity? Task { get; set; }
        public int? TaskId { get; set; }

        [ForeignKey("UserId")]
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int? UserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
