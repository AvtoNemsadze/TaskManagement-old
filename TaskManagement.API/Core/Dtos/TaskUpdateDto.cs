using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Dtos
{
    public class TaskUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
    }
}
