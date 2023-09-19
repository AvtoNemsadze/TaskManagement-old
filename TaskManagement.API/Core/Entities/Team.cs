using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagement.API.Core.Entities
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        // Creator information
        //[ForeignKey("CreatorId")]
        //public ApplicationUser Creator { get; set; } // Navigation property to refer to the Creator
        //public int CreatorId { get; set; }

        // realtionship with other objects
        [JsonIgnore]
        public ICollection<ApplicationUser> Members { get; set; } = new List<ApplicationUser>();
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
