﻿namespace TaskManagement.API.Core.Dtos
{
    public class TaskGetDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int UserId { get; set; }
        public string? AttachFile { get; set; }
    }
}
