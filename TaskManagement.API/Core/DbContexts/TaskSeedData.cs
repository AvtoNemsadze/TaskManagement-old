using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Enums;
using TaskStatus = TaskManagement.API.Core.Enums.TaskStatus;

namespace TaskManagement.API.Core.DataAccess
{
    public class TaskSeedData
    {
        private readonly ApplicationDbContext _context;

        public TaskSeedData(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void SeedData()
        {
            if (!_context.Tasks.Any() && _context.Users.Any())
            {
                var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Title = "Complete Project Proposal",
                    Description = "Prepare and submit the project proposal by end of the week.",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    Status = TaskStatus.NotStarted.ToString(),
                    Priority = TaskPriority.Medium.ToString(),
                    UserId = 1,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Review Code Changes",
                    Description = "Review and provide feedback on the latest code changes.",
                    DueDate = DateTime.UtcNow.AddDays(2),
                    Status = TaskStatus.InProgress.ToString(),
                    Priority = TaskPriority.High.ToString(),
                    UserId = 2,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Client Meeting",
                    Description = "Schedule a meeting with the client to discuss project progress.",
                    DueDate = DateTime.UtcNow.AddDays(3),
                    Status = TaskStatus.Started.ToString(),
                    Priority = TaskPriority.Medium.ToString(),
                    UserId = 1,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Prepare Presentation",
                    Description = "Prepare a presentation for the upcoming team meeting.",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = TaskStatus.NotStarted.ToString(),
                    Priority = TaskPriority.Low.ToString(),
                    UserId = 2,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Update User Documentation",
                    Description = "Update the user documentation with the latest features and changes.",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    Status = TaskStatus.NotStarted.ToString(),
                    Priority = TaskPriority.Medium.ToString(),
                    UserId = 1,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Fix Critical Bug",
                    Description = "Identify and fix a critical bug affecting user login.",
                    DueDate = DateTime.UtcNow.AddDays(2),
                    Status = TaskStatus.InProgress.ToString(),
                    Priority = TaskPriority.High.ToString(),
                    UserId = 2,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Implement Email Notifications",
                    Description = "Add email notifications for important events.",
                    DueDate = DateTime.UtcNow.AddDays(14),
                    Status = TaskStatus.NotStarted.ToString(),
                    Priority = TaskPriority.High.ToString(),
                    UserId = 1,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Optimize Database Queries",
                    Description = "Identify and optimize slow-performing database queries.",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    Status = TaskStatus.InProgress.ToString(),
                    Priority = TaskPriority.Medium.ToString(),
                    UserId = 1,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Create User Onboarding Video",
                    Description = "Create a video tutorial for new users to get started.",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = TaskStatus.Started.ToString(),
                    Priority = TaskPriority.Low.ToString(),
                    UserId = 2,
                    //CreatedByUserId = 30
                },
                new TaskEntity
                {
                    Title = "Create Database Schema",
                    Description = "Create Database Schema and optimize slow-performing database operations",
                    DueDate = DateTime.UtcNow.AddDays(8),
                    Status = TaskStatus.Started.ToString(),
                    Priority = TaskPriority.Low.ToString(),
                    UserId = 2,
                    //CreatedByUserId = 30
                },
            };
                _context.Tasks.AddRange(tasks);
                _context.SaveChanges();
            }
        }
    }
}


